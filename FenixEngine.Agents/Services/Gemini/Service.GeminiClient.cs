using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FenixEngine.Agents.Utils;

namespace FenixEngine.Agents.Services.Gemini
{
    public class ServiceGeminiClient : IGeminiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://generativelanguage.googleapis.com/v1beta";

        public ServiceGeminiClient()
        {
            _httpClient = HttpClientFactoryHelper.Instance;
        }

        public async Task<string> PostJsonAsync(string endpoint, object payload, string? apiKey = null)
        {
            var url = $"{_baseUrl}/{endpoint}?key={apiKey}";
            var json = JsonSerializerHelper.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAsync(string endpoint, string? apiKey = null)
        {
            var url = $"{_baseUrl}/{endpoint}?key={apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
