using FenixEngine.Services.Src.Workflow;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.AppCore.Controllers;

public enum WorkflowExecutionState
{
    Idle,
    Prompting,
    Generating,
    Validating,
    Completed,
    Error
}

public class WorkflowController
{
    private readonly IPromptExecutionService _promptExecutionService;
    private readonly IRupDocumentService _rupDocumentService;
    private readonly IArchitectureProjectService _architectureProjectService;

    public WorkflowController(
        IPromptExecutionService promptExecutionService,
        IRupDocumentService rupDocumentService,
        IArchitectureProjectService architectureProjectService)
    {
        _promptExecutionService = promptExecutionService;
        _rupDocumentService = rupDocumentService;
        _architectureProjectService = architectureProjectService;
    }

    public WorkflowExecutionState CurrentState { get; private set; } = WorkflowExecutionState.Idle;

    public async Task<WorkflowExecutionState> RunAsync(string actionName)
    {
        return await ExecuteAsync(actionName, actionName, null);
    }

    public async Task<WorkflowExecutionState> ExecuteAsync(
        string actionName,
        string prompt,
        string? projectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            CurrentState = WorkflowExecutionState.Prompting;

            if (actionName.Equals("rup", StringComparison.OrdinalIgnoreCase))
            {
                CurrentState = WorkflowExecutionState.Generating;
                await _rupDocumentService.GenerateDocumentAsync(projectName ?? "General", prompt, cancellationToken);
            }
            else if (actionName.Equals("architecture", StringComparison.OrdinalIgnoreCase))
            {
                CurrentState = WorkflowExecutionState.Generating;
                await _architectureProjectService.GenerateProjectStructureAsync(
                    projectName ?? "NewProject",
                    "MAUI + .NET",
                    prompt,
                    cancellationToken);
            }
            else
            {
                CurrentState = WorkflowExecutionState.Generating;
                await _promptExecutionService.ExecutePromptAsync(prompt, projectName, cancellationToken);
            }

            CurrentState = WorkflowExecutionState.Validating;
            CurrentState = WorkflowExecutionState.Completed;
        }
        catch
        {
            CurrentState = WorkflowExecutionState.Error;
        }

        return CurrentState;
    }
}
