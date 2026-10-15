using System.Text;
using System.Text.Json;

namespace FenixEngine.Services.Src.ApiClient;

public interface IHttpClientService
{
    Task<string> PostJsonAsync(string url, object payload, string? apiKey = null );
    Task<T?> GetJsonAsync<T>(string url, string? apiKey = null);
    Task<string> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
}