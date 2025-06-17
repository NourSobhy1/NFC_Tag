
using PCSC;
using PCSC.Iso7816;
using System;
using System.Diagnostics;
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

			Console.WriteLine($"Listening for NFC tags on reader: {readerName}");

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
					var recvPci = new SCardPCI();
					var receiveBuffer = new byte[256];
					var receiveLength = receiveBuffer.Length;

					var result = reader.Transmit(sendPci, apdu.ToArray(), apdu.ToArray().Length,
												 recvPci, receiveBuffer, ref receiveLength);

					if (result == SCardError.Success)
					{
						var uid = BitConverter.ToString(receiveBuffer, 0, receiveLength).Replace("-", "");
						if (uid != lastUid)
						{
							lastUid = uid;
							Console.WriteLine($"Card UID: {uid}");

							// Read block 4 to check if URL already exists
							var readCommand = new byte[] {
								0xFF, 0xB0, 0x00, 0x04, 0x10
							};
							var readResp = new byte[258];
							var readLen = readResp.Length;
							recvPci = new SCardPCI();

							result = reader.Transmit(sendPci, readCommand, readCommand.Length,
													 recvPci, readResp, ref readLen);

							if (result == SCardError.Success && readResp[readLen - 2] == 0x90)
							{
								var dataBytes = new byte[readLen - 2];
								Array.Copy(readResp, 0, dataBytes, 0, readLen - 2);
								var link = Encoding.ASCII.GetString(dataBytes).Trim('\0');

								if (!string.IsNullOrWhiteSpace(link))
								{
									Console.WriteLine($"Link on card: {link}");
									try
									{
										Process.Start(new ProcessStartInfo
										{
											FileName = link,
											UseShellExecute = true
										});
									}
									catch (Exception ex)
									{
										Console.WriteLine($"Failed to open browser: {ex.Message}");
									}
								}
								else
								{
									Console.Write("No link found. Do you want to write one? (y/n): ");
									var input = Console.ReadLine()?.Trim().ToLower();

									if (input == "y")
									{
										Console.Write("Enter URL to write to the card: ");
										var url = Console.ReadLine();
										var data = Encoding.ASCII.GetBytes(url ?? "");

										if (data.Length > 16)
										{
											Console.WriteLine("Data too long! Must be 16 bytes max.");
										}
										else
										{
											Array.Resize(ref data, 16); // pad with 0x00

											// Authenticate block 4
											var authCommand = new byte[] {
												0xFF, 0x86, 0x00, 0x00, 0x05,
												0x01, 0x00, 0x04, 0x60, 0x00
											};

											var authResp = new byte[2];
											var authLen = authResp.Length;
											recvPci = new SCardPCI();

											result = reader.Transmit(sendPci, authCommand, authCommand.Length,
																	 recvPci, authResp, ref authLen);

											if (result == SCardError.Success && authResp[0] == 0x90)
											{
												var writeCommand = new byte[21];
												writeCommand[0] = 0xFF;
												writeCommand[1] = 0xD6;
												writeCommand[2] = 0x00;
												writeCommand[3] = 0x04;
												writeCommand[4] = 0x10;
												Array.Copy(data, 0, writeCommand, 5, 16);

												var writeResp = new byte[2];
												var writeLen = writeResp.Length;
												recvPci = new SCardPCI();

												result = reader.Transmit(sendPci, writeCommand, writeCommand.Length,
																		 recvPci, writeResp, ref writeLen);

												if (result == SCardError.Success && writeResp[0] == 0x90)
												{
													Console.WriteLine("Data written to card successfully.");
												}
												else
												{
													Console.WriteLine("Failed to write data to card.");
												}
											}
											else
											{
												Console.WriteLine("Authentication failed.");
											}
										}
									}
								}
							}
						}
					}

					reader.Disconnect(SCardReaderDisposition.Leave);
				}

				await Task.Delay(1000); // small delay to prevent high CPU usage
			}
		}
	}
}
