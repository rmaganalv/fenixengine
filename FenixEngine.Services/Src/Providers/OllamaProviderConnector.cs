using FenixEngine.Services.Src.ApiClient;
using FenixEngine.Shared.Src.Models;

namespace FenixEngine.Services.Src.Providers;

public sealed class OllamaProviderConnector : ProviderConnectorBase
{
    public OllamaProviderConnector(IHttpClientService httpClientService) : base(httpClientService) { }

    public override string ProviderType => AiProviderTypes.Ollama;

    public override async Task TestConnectionAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default)
    {
        using var document = await GetJsonAsync(NativeBaseUrl(provider) + "/api/tags", apiKey, "Authorization", cancellationToken);
        if (!document.RootElement.TryGetProperty("models", out var models) || models.ValueKind != System.Text.Json.JsonValueKind.Array)
            throw new InvalidOperationException("Ollama respondió con un catálogo de modelos inesperado.");
    }

    public override async Task<IReadOnlyList<DiscoveredLlmModel>> GetModelsAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default)
    {
        using var document = await GetJsonAsync(NativeBaseUrl(provider) + "/api/tags", apiKey, "Authorization", cancellationToken);
        if (!document.RootElement.TryGetProperty("models", out var models) || models.ValueKind != System.Text.Json.JsonValueKind.Array)
            return Array.Empty<DiscoveredLlmModel>();

        return models.EnumerateArray()
            .Where(model => model.TryGetProperty("name", out _))
            .Select(model => model.GetProperty("name").GetString())
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => new DiscoveredLlmModel(name!, name!))
            .ToArray();
    }

    public override async Task<string> ExecutePromptAsync(AiProvider provider, string modelIdentifier, string systemPrompt, string prompt, string? apiKey, CancellationToken cancellationToken = default)
    {
        using var document = await PostJsonAsync(NativeBaseUrl(provider) + "/api/chat", new
        {
            model = modelIdentifier,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = prompt }
            },
            stream = false
        }, apiKey, "Authorization", cancellationToken);

        return document.RootElement.GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
    }

    private static string NativeBaseUrl(AiProvider provider)
    {
        var baseUrl = BaseUrl(provider);
        return baseUrl.EndsWith("/v1", StringComparison.OrdinalIgnoreCase) ? baseUrl[..^3] : baseUrl;
    }
}