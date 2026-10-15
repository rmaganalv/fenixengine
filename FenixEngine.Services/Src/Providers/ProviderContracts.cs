using FenixEngine.Shared.Src.Models;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Services.Src.Providers;

public static class AiProviderTypes
{
    public const string OpenAiCompatible = "OpenAI Compatible";
    public const string Gemini = "Gemini";
    public const string Ollama = "Ollama";
}

public sealed record DiscoveredLlmModel(string Name, string ModelIdentifier, int? ContextWindow = null);

public interface IAProviderConnector
{
    string ProviderType { get; }
    Task TestConnectionAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DiscoveredLlmModel>> GetModelsAsync(AiProvider provider, string? apiKey, CancellationToken cancellationToken = default);
    Task<string> ExecutePromptAsync(
        AiProvider provider,
        string modelIdentifier,
        string systemPrompt,
        string prompt,
        string? apiKey,
        CancellationToken cancellationToken = default);
}

public interface IAProviderConnectorFactory
{
    IAProviderConnector GetConnector(string providerType);
}

public interface IApiKeyStore
{
    Task SetAsync(string providerId, string apiKey);
    Task<string?> GetAsync(string providerId);
    Task RemoveAsync(string providerId);
}

public sealed class SecureApiKeyStore : IApiKeyStore
{
    private const string KeyPrefix = "fenix.ai-provider.";

    public Task SetAsync(string providerId, string apiKey) =>
        SecureStorage.Default.SetAsync(KeyPrefix + providerId, apiKey);

    public Task<string?> GetAsync(string providerId) =>
        SecureStorage.Default.GetAsync(KeyPrefix + providerId);

    public Task RemoveAsync(string providerId)
    {
        SecureStorage.Default.Remove(KeyPrefix + providerId);
        return Task.CompletedTask;
    }
}

public sealed class ProviderConnectorFactory : IAProviderConnectorFactory
{
    private readonly IReadOnlyDictionary<string, IAProviderConnector> _connectors;

    public ProviderConnectorFactory(IEnumerable<IAProviderConnector> connectors)
    {
        _connectors = connectors.ToDictionary(connector => connector.ProviderType, StringComparer.OrdinalIgnoreCase);
    }

    public IAProviderConnector GetConnector(string providerType) =>
        _connectors.TryGetValue(providerType, out var connector)
            ? connector
            : throw new NotSupportedException($"No hay un conector registrado para el tipo '{providerType}'.");
}