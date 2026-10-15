using FenixEngine.Services.Src.ApiClient;
using FenixEngine.Shared.Src.Models;

namespace FenixEngine.Services.Src.Providers;

public sealed class GeminiProviderConnector : ProviderConnectorBase
{
    public GeminiProviderConnector(IHttpClientService httpClientService) : base(httpClientService) { }

    public override string ProviderType => AiProviderTypes.Gemini;

    public override async Task TestConnectionAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default)
    {
        using var document = await GetJsonAsync(BaseUrl(provider) + "/v1beta/models", apiKey, "x-goog-api-key", cancellationToken);
        if (!document.RootElement.TryGetProperty("models", out var models) || models.ValueKind != System.Text.Json.JsonValueKind.Array)
            throw new InvalidOperationException("Gemini respondió con un catálogo de modelos inesperado.");
    }

    public override async Task<IReadOnlyList<DiscoveredLlmModel>> GetModelsAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default)
    {
        using var document = await GetJsonAsync(BaseUrl(provider) + "/v1beta/models", apiKey, "x-goog-api-key", cancellationToken);
        if (!document.RootElement.TryGetProperty("models", out var models) || models.ValueKind != System.Text.Json.JsonValueKind.Array)
            return Array.Empty<DiscoveredLlmModel>();

        return models.EnumerateArray()
            .Where(model => !model.TryGetProperty("supportedGenerationMethods", out var methods)
                || methods.EnumerateArray().Any(method => method.GetString() == "generateContent"))
            .Where(model => model.TryGetProperty("name", out _))
            .Select(model => model.GetProperty("name").GetString())
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => new DiscoveredLlmModel(name!, name!))
            .ToArray();
    }

    public override async Task<string> ExecutePromptAsync(AiProvider provider, string modelIdentifier, string systemPrompt, string prompt, string? apiKey, CancellationToken cancellationToken = default)
    {
        var model = modelIdentifier.StartsWith("models/", StringComparison.OrdinalIgnoreCase)
            ? modelIdentifier["models/".Length..]
            : modelIdentifier;
        var url = $"{BaseUrl(provider)}/v1beta/models/{Uri.EscapeDataString(model)}:generateContent";
        using var document = await PostJsonAsync(url, new
        {
            systemInstruction = new { parts = new[] { new { text = systemPrompt } } },
            contents = new[] { new { role = "user", parts = new[] { new { text = prompt } } } }
        }, apiKey, "x-goog-api-key", cancellationToken);

        return document.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString() ?? string.Empty;
    }
}