using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FenixEngine.Shared.Services;


namespace FenixEngine.Shared.Services.IaProviders;

public class AiExternalService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AiExternalService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    /// <summary>
    /// Genera texto enviando un prompt a la API de Gemini (Ej: gemini-2.5-flash).
    /// Endpoint: POST /v1beta/models/{model}:generateContent
    /// </summary>
    public async Task<string?> GenerateContentAsync(string BaseUrl, string prompt, string model)
    {
        var endpoint = $"{BaseUrl}{model}:generateContent?key={_apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var jsonPayload = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        
        using var doc = JsonDocument.Parse(jsonResponse);
        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return text;
    }

    public Task<string> GetExternalAsync(string BaseUrl, string endpoint, string? apiKey = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> PostJsonAsync(string endpoint, object payload, string? apiKey = null)
    {
        throw new NotImplementedException();
    }
}
