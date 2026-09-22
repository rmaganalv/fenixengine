using FenixEngine.Services.Src.Models;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Services.Src.Workflow;

public interface IPromptExecutionService
{
    Task<string> ExecutePromptAsync(string prompt, string? projectName = null, CancellationToken cancellationToken = default);
}

public interface IRupDocumentService
{
    Task<string> GenerateDocumentAsync(string projectName, string prompt, CancellationToken cancellationToken = default);
}

public interface IArchitectureProjectService
{
    Task<string> GenerateProjectStructureAsync(string projectName, string stack, string prompt, CancellationToken cancellationToken = default);
}

public static class WorkflowServiceDefaults
{
    public const string DefaultModel = "llama3";
    public const string DefaultBaseUrl = "http://127.0.0.1:11434/v1/";
}
