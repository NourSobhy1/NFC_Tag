using NFC.Application.Interfaces.IServices;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace NFC.Application.Services
{
    public class ChatService : IChatService
    {

        private readonly HttpClient _httpClient;

        public ChatService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("OpenRouterClient");
        }
        public async Task<string> AskAsync(string question)
        {
            var body = new
            {
                model = "deepseek/deepseek-r1:free",
                messages = new[]
           {
                new { role = "user", content = question }
            }
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("chat/completions", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var result = doc.RootElement
                            .GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString();

            return result ?? "No response.";
        }
    }
}
