using FenixEngine.Services.Src.ApiClient;
using FenixEngine.Services.Src.Models;
using FenixEngine.Shared.Src.Services;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Services.Src.Workflow;

public class PromptExecutionService : IPromptExecutionService
{
    private readonly IHttpClientService _httpClientService;
    private readonly IFileService _fileService;
    private readonly IFolderService _folderService;
    private readonly ITerminalService _terminalService;
    private readonly ILoggerService _loggerService;

    public PromptExecutionService(
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

    public async Task<string> ExecutePromptAsync(string prompt, string? projectName = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("El prompt no puede estar vacío.", nameof(prompt));

        _loggerService.Info($"Ejecutando prompt para proyecto '{projectName ?? "general"}'");

        var options = new AgentOptions
        {
            BaseUrl = WorkflowServiceDefaults.DefaultBaseUrl,
            SelectedModel = WorkflowServiceDefaults.DefaultModel,
            Temperature = 0.3,
            MaxTokens = 2048
        };

        var payload = new
        {
            model = options.SelectedModel,
            messages = new[]
            {
                new { role = "system", content = "Eres un asistente senior de arquitectura de software y desarrollo." },
                new { role = "user", content = prompt }
            },
            temperature = options.Temperature,
            max_tokens = options.MaxTokens,
            stream = false
        };

        var result = await _httpClientService.PostJsonAsync(options.BaseUrl.TrimEnd('/') + "/chat/completions", payload, options.ApiKey);
        return result;
    }
}
