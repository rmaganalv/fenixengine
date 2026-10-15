using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FenixEngine.Services.Src.ApiClient;
using FenixEngine.Shared.Src.Models;

namespace FenixEngine.Services.Src.Providers;

public abstract class ProviderConnectorBase : IAProviderConnector
{
    private readonly IHttpClientService _httpClientService;

    protected ProviderConnectorBase(IHttpClientService httpClientService) => _httpClientService = httpClientService;

    public abstract string ProviderType { get; }
    public abstract Task TestConnectionAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default);
    public abstract Task<IReadOnlyList<DiscoveredLlmModel>> GetModelsAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default);
    public abstract Task<string> ExecutePromptAsync(AiProvider provider, string modelIdentifier, string systemPrompt, string prompt, string? apiKey, CancellationToken cancellationToken = default);

    protected async Task<JsonDocument> GetJsonAsync(string url, string? apiKey, string? apiKeyHeader, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddApiKey(request, apiKey, apiKeyHeader);
        return JsonDocument.Parse(await _httpClientService.SendAsync(request, cancellationToken));
    }

    protected async Task<JsonDocument> PostJsonAsync(string url, object payload, string? apiKey, string? apiKeyHeader, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };
        AddApiKey(request, apiKey, apiKeyHeader);
        return JsonDocument.Parse(await _httpClientService.SendAsync(request, cancellationToken));
    }

    protected static void AddApiKey(HttpRequestMessage request, string? apiKey, string? apiKeyHeader)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) return;

        if (string.Equals(apiKeyHeader, "Authorization", StringComparison.OrdinalIgnoreCase))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        else if (!string.IsNullOrWhiteSpace(apiKeyHeader))
            request.Headers.TryAddWithoutValidation(apiKeyHeader, apiKey);
    }

    protected static string BaseUrl(AiProvider provider) => provider.BaseUrl.TrimEnd('/');
}