using FenixEngine.Services.Src.ApiClient;
using FenixEngine.Shared.Src.Services;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Services.Src.Workflow;

public class RupDocumentService : IRupDocumentService
{
    private readonly IHttpClientService _httpClientService;
    private readonly IFileService _fileService;
    private readonly IFolderService _folderService;
    private readonly ITerminalService _terminalService;
    private readonly ILoggerService _loggerService;

    public RupDocumentService(
        IHttpClientService httpClientService,
        IFileService fileService,
        IFolderService folderService,
        ITerminalService terminalService,
        ILoggerService loggerService)
    {
        _httpClientService = httpClientService;
        _fileService = fileService;
        _folderService = folderService;
        _terminalService = terminalService;
        _loggerService = loggerService;
    }

    public async Task<string> GenerateDocumentAsync(string projectName, string prompt, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(projectName)) throw new ArgumentException("Project name is required.", nameof(projectName));
        if (string.IsNullOrWhiteSpace(prompt)) throw new ArgumentException("Prompt is required.", nameof(prompt));

        var options = new
        {
            model = WorkflowServiceDefaults.DefaultModel,
            system = "Genera un documento RUP para un proyecto de software con alcance, requisitos, objetivos, arquitectura, riesgos y cronograma.",
            prompt
        };

        var response = await _httpClientService.PostJsonAsync(WorkflowServiceDefaults.DefaultBaseUrl.TrimEnd('/') + "/chat/completions", options, null);

        var outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FenixEngine", "Projects", projectName, "docs");
        await _folderService.CreateFolder(outputFolder);

        var filePath = Path.Combine(outputFolder, "RUP-Document.md");
        await _fileService.CreateFile(filePath);
        await _fileService.EditFile(filePath, response);

        _loggerService.Info($"Documento RUP generado en '{filePath}'");
        return filePath;
    }
}
