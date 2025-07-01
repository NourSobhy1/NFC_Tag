using PCSC;
using PCSC.Iso7816;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFCReaderClient
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var contextFactory = ContextFactory.Instance;
			using var context = contextFactory.Establish(SCardScope.System);

			var readerNames = context.GetReaders();
			if (readerNames == null || readerNames.Length == 0)
			{
				Console.WriteLine("No smart card readers found.");
				return;
			}

			var readerName = readerNames[0];
			using var reader = new SCardReader(context);

			Console.WriteLine($"📡 Listening for NFC tags on reader: {readerName}");

			string lastUid = null;

			while (true)
			{
				var connectResult = reader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.Any);
				if (connectResult == SCardError.Success)
				{
					var apdu = new CommandApdu(IsoCase.Case2Short, reader.ActiveProtocol)
					{
						CLA = 0xFF,
						INS = 0xCA,
						P1 = 0x00,
						P2 = 0x00,
						Le = 0x00
					};

					var sendPci = SCardPCI.GetPci(reader.ActiveProtocol);
					var receiveBuffer = new byte[256];
					var receiveLength = receiveBuffer.Length;

					var result = reader.Transmit(sendPci, apdu.ToArray(), apdu.ToArray().Length,
												 new SCardPCI(), receiveBuffer, ref receiveLength);

					if (result == SCardError.Success)
					{
						var uid = BitConverter.ToString(receiveBuffer, 0, receiveLength).Replace("-", "");
						if (uid != lastUid)
						{
							lastUid = uid;
							Console.WriteLine($"💳 Card UID: {uid}");

							var url = $"https://touchgo2.netlify.app/scan.html?nfc={uid}";

							try
							{
								Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
								{
									CreateNoWindow = true,
									UseShellExecute = false
								});
								Console.WriteLine($"🌐 Opened URL: {url}");
							}
							catch (Exception ex)
							{
								Console.WriteLine($"❌ Failed to open browser: {ex.Message}");
							}

							Console.Write("📝 Do you want to write this URL to the card (for mobile use)? (y/n): ");
							var input = Console.ReadLine()?.Trim().ToLower();

							if (input == "y")
							{
								var data = Encoding.ASCII.GetBytes(url);
								var block4 = data.Skip(0).Take(16).ToArray();
								var block5 = data.Skip(16).Take(16).ToArray();
								var block6 = data.Skip(32).Take(16).ToArray();

								Array.Resize(ref block4, 16);
								Array.Resize(ref block5, 16);
								Array.Resize(ref block6, 16);

								// المفاتيح اللي هنحاول بيها
								byte[][] commonKeys =
								{
									new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
									new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF },
									new byte[] { 0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5 }
								};

								bool authenticated = false;
								foreach (var key in commonKeys)
								{
									var authCommand = new byte[]
									{
										0xFF, 0x86, 0x00, 0x00, 0x05,
										0x01, 0x00, 0x04, 0x60, 0x00
									};

									// أضف المفتاح نفسه في نهاية الأمر
									authCommand = authCommand.Concat(key).ToArray();

									var authResp = new byte[2];
									var authLen = authResp.Length;

									var authResult = reader.Transmit(sendPci, authCommand, authCommand.Length,
																	 new SCardPCI(), authResp, ref authLen);

									if (authResult == SCardError.Success && authResp[0] == 0x90)
									{
										authenticated = true;
										Console.WriteLine("🔐 Authentication succeeded with key: " +
															BitConverter.ToString(key));
										break;
									}
								}

								if (!authenticated)
								{
									Console.WriteLine("❌ Authentication failed with all common keys.");
									goto SkipWrite;
								}

								// دالة كتابة بلوك
								void WriteBlock(byte blockNumber, byte[] blockData)
								{
									var cmd = new byte[21];
									cmd[0] = 0xFF;
									cmd[1] = 0xD6;
									cmd[2] = 0x00;
									cmd[3] = blockNumber;
									cmd[4] = 0x10;
									Array.Copy(blockData, 0, cmd, 5, 16);

									var resp = new byte[2];
									var len = resp.Length;

									var writeResult = reader.Transmit(sendPci, cmd, cmd.Length,
																	  new SCardPCI(), resp, ref len);

									if (writeResult == SCardError.Success && resp[0] == 0x90)
										Console.WriteLine($"✅ Block {blockNumber} written.");
									else
										Console.WriteLine($"❌ Failed to write Block {blockNumber}.");
								}

								WriteBlock(4, block4);
								WriteBlock(5, block5);
								WriteBlock(6, block6);
							}

						SkipWrite:;
						}
					}
					reader.Disconnect(SCardReaderDisposition.Leave);
				}

				await Task.Delay(1000);
			}
		}
	}
}

