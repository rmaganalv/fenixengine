
using System.Net.Http.Headers;
using System.Text;
using FenixEngine.Agents.Utils;


namespace FenixEngine.Agents.Services.Ollama;
public class ServiceOllamaClient : IOllamaClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:11434/api";

    public ServiceOllamaClient()
    {
        _httpClient = HttpClientFactoryHelper.Instance;;
    }

    public async Task<string> PostJsonAsync(string endpoint, object payload, string? apiKey = null)
    {
        var url = $"{_baseUrl}/{endpoint}";
        var json = JsonSerializerHelper.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        if (!string.IsNullOrEmpty(apiKey))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }


    public async Task<string> GetAsync(string endpoint, string? apiKey = null)
    {
        var url = $"{_baseUrl}/{endpoint}";

        if (!string.IsNullOrEmpty(apiKey))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
