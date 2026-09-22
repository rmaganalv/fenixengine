using FenixEngine.Services.Src.Workflow;
using FenixEngine.Shared;
using FenixEngine.AppCore.Src.Services;
using FenixEngine.AppCore.Controllers;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.AppCore;

public static class AppCoreExtension
{
    public static MauiAppBuilder UseEngineAppCore(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IPromptExecutionService, PromptExecutionService>();
        builder.Services.AddSingleton<IRupDocumentService, RupDocumentService>();
        builder.Services.AddSingleton<IArchitectureProjectService, ArchitectureProjectService>();
        builder.Services.AddScoped<ProjectDataService>();
        builder.Services.AddScoped<ProjectPortfolioController>();
        builder.Services.AddScoped<AuthenticationService>();
        builder.Services.AddScoped<SettingsDataService>();
        builder.Services.AddScoped<WorkflowController>();
        builder.UseSharedServices();
        return builder;
    }
}