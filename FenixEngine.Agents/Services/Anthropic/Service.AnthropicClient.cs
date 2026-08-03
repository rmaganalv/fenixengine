using System.Net.Http.Headers;
using System.Text;
using FenixEngine.Agents.Services;
using FenixEngine.Agents.Utils;

namespace Service.Agents.Services.Anthropic
{
    public class ServiceAnthropicClient : IAnthropicClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.anthropic.com/v1";

        public ServiceAnthropicClient()
        {
            _httpClient = HttpClientFactoryHelper.Instance;
        }

        public async Task<string> PostJsonAsync(string endpoint, object payload, string apiKey)
        {
            var url = $"{_baseUrl}/{endpoint}";
            var json = JsonSerializerHelper.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAsync(string endpoint, string apiKey)
        {
            var url = $"{_baseUrl}/{endpoint}";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
