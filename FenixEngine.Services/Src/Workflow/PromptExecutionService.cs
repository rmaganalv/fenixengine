using FenixEngine.Services.Src.Providers;
using FenixEngine.Shared.Src.Models;
using FenixEngine.Shared.Src.Services;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Services.Src.Workflow;

public class PromptExecutionService : IPromptExecutionService
{
    private readonly IAProviderConnectorFactory _connectorFactory;
    private readonly IApiKeyStore _apiKeyStore;
    private readonly ILoggerService _loggerService;

    public PromptExecutionService(
        IAProviderConnectorFactory connectorFactory,
        IApiKeyStore apiKeyStore,
        ILoggerService loggerService)
    {
        _connectorFactory = connectorFactory;
        _apiKeyStore = apiKeyStore;
        _loggerService = loggerService;
    }

    public async Task<string> ExecutePromptAsync(
        AiProvider provider,
        string modelIdentifier,
        string prompt,
        string? projectName = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("El prompt no puede estar vacío.", nameof(prompt));
        if (!provider.IsEnabled || string.IsNullOrWhiteSpace(modelIdentifier))
            throw new InvalidOperationException("Selecciona un proveedor y un modelo habilitados.");

        _loggerService.Info($"Ejecutando prompt para proyecto '{projectName ?? "general"}'");
        var apiKey = await _apiKeyStore.GetAsync(provider.Id);
        if (!provider.IsLocal && string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("El proveedor seleccionado no tiene una API key configurada.");

        return await _connectorFactory.GetConnector(provider.ProviderType).ExecutePromptAsync(
            provider,
            modelIdentifier,
            "Eres un asistente de desarrollo de software.",
            prompt,
            apiKey,
            cancellationToken);
    }
}
