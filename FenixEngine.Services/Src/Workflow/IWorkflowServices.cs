using FenixEngine.Shared.Src.Models;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Services.Src.Workflow;

public interface IPromptExecutionService
{
    Task<string> ExecutePromptAsync(
        AiProvider provider,
        string modelIdentifier,
        string prompt,
        string? projectName = null,
        CancellationToken cancellationToken = default);
}

public interface IRupDocumentService
{
    Task<string> GenerateDocumentAsync(
        AiProvider provider,
        string modelIdentifier,
        string projectName,
        string prompt,
        CancellationToken cancellationToken = default);
}

public interface IArchitectureProjectService
{
    Task<string> GenerateProjectStructureAsync(string projectName, string stack, string prompt, CancellationToken cancellationToken = default);
}
