using System.Text;
using System.Text.Json;

namespace FenixEngine.Services.Src.ApiClient;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    // Usamos una única instancia estática de HttpClient optimizada para evitar
    // la latencia de inicialización del Handler de DI en Mac Catalyst/iOS
    private static readonly HttpClient SharedClient = new HttpClient
    {
        Timeout = TimeSpan.FromMinutes(3)
    };

    public HttpClientService()
    {
        _httpClient = SharedClient;
    }

    public async Task<string> PostJsonAsync(string url, object payload, string? apiKey)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload);
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en POST ({url}): {ex.Message}", ex);
        }
    }

    public async Task<T?> GetJsonAsync<T>(string url, string? apiKey)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            }

            // Al usar GetAsync directo se evita la inicialización perezosa de sockets en GET
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GET ({url}): {ex.Message}", ex);
        }
    }
}
