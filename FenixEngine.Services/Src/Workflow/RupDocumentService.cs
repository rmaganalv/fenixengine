using FenixEngine.Services.Src.Providers;
using FenixEngine.Shared.Src.Models;
using FenixEngine.Shared.Src.Services;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Services.Src.Workflow;

public class RupDocumentService : IRupDocumentService
{
    private readonly IAProviderConnectorFactory _connectorFactory;
    private readonly IApiKeyStore _apiKeyStore;
    private readonly IFileService _fileService;
    private readonly IFolderService _folderService;
    private readonly ITerminalService _terminalService;
    private readonly ILoggerService _loggerService;

    public RupDocumentService(
        IAProviderConnectorFactory connectorFactory,
        IApiKeyStore apiKeyStore,
        IFileService fileService,
        IFolderService folderService,
        ITerminalService terminalService,
        ILoggerService loggerService)
    {
        _connectorFactory = connectorFactory;
        _apiKeyStore = apiKeyStore;
        _fileService = fileService;
        _folderService = folderService;
        _terminalService = terminalService;
        _loggerService = loggerService;
    }

    public async Task<string> GenerateDocumentAsync(
        AiProvider provider,
        string modelIdentifier,
        string projectName,
        string prompt,
        CancellationToken cancellationToken = default)
    {
        if (!provider.IsEnabled || string.IsNullOrWhiteSpace(modelIdentifier))
            throw new InvalidOperationException("Selecciona un proveedor y un modelo habilitados.");
        if (string.IsNullOrWhiteSpace(projectName)) throw new ArgumentException("Project name is required.", nameof(projectName));
        if (string.IsNullOrWhiteSpace(prompt)) throw new ArgumentException("Prompt is required.", nameof(prompt));

        var apiKey = await _apiKeyStore.GetAsync(provider.Id);
        if (!provider.IsLocal && string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("El proveedor seleccionado no tiene una API key configurada.");
        var response = await _connectorFactory.GetConnector(provider.ProviderType).ExecutePromptAsync(
            provider,
            modelIdentifier,
            "Genera un documento RUP para un proyecto de software.",
            prompt,
            apiKey,
            cancellationToken);

        var outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FenixEngine", "Projects", projectName, "docs");
        await _folderService.CreateFolder(outputFolder);

        var filePath = Path.Combine(outputFolder, "RUP-Document.md");
        await _fileService.CreateFile(filePath);
        await _fileService.EditFile(filePath, response);

        _loggerService.Info($"Documento RUP generado en '{filePath}'");
        return filePath;
    }
}
