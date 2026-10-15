using FenixEngine.Services.Src.ApiClient;
using FenixEngine.Shared.Src.Models;

namespace FenixEngine.Services.Src.Providers;

public sealed class OpenAiProviderConnector : ProviderConnectorBase
{
    public OpenAiProviderConnector(IHttpClientService httpClientService) : base(httpClientService) { }

    public override string ProviderType => AiProviderTypes.OpenAiCompatible;

    public override async Task TestConnectionAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default)
    {
        using var document = await GetJsonAsync(BaseUrl(provider) + "/models", apiKey, "Authorization", cancellationToken);
        if (!document.RootElement.TryGetProperty("data", out var models) || models.ValueKind != System.Text.Json.JsonValueKind.Array)
            throw new InvalidOperationException("El proveedor respondió, pero no devolvió un catálogo de modelos compatible.");
    }

    public override async Task<IReadOnlyList<DiscoveredLlmModel>> GetModelsAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default)
    {
        using var document = await GetJsonAsync(BaseUrl(provider) + "/models", apiKey, "Authorization", cancellationToken);
        if (!document.RootElement.TryGetProperty("data", out var models) || models.ValueKind != System.Text.Json.JsonValueKind.Array)
            return Array.Empty<DiscoveredLlmModel>();

        return models.EnumerateArray()
            .Where(item => item.TryGetProperty("id", out _))
            .Select(item => item.GetProperty("id").GetString())
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => new DiscoveredLlmModel(id!, id!))
            .ToArray();
    }

    public override async Task<string> ExecutePromptAsync(AiProvider provider, string modelIdentifier, string systemPrompt, string prompt, string? apiKey, CancellationToken cancellationToken = default)
    {
        using var document = await PostJsonAsync(BaseUrl(provider) + "/chat/completions", new
        {
            model = modelIdentifier,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = prompt }
            },
            stream = false
        }, apiKey, "Authorization", cancellationToken);

        return document.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
    }
}