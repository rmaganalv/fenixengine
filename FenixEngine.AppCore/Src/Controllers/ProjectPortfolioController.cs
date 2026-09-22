using FenixEngine.AppCore.Src.Services;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.AppCore.Controllers;

public sealed class ProjectSummary
{
    public string Name { get; set; } = string.Empty;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = "Local";
    public string Status { get; set; } = "Active";
}

public sealed class ProjectActivity
{
    public string Title { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = "Update";
}

public sealed class ProjectTask
{
    public string ProjectName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string State { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class ProjectTaskGroup : List<ProjectTask>
{
    public string ProjectName { get; }

    public ProjectTaskGroup(string projectName, IEnumerable<ProjectTask> tasks)
        : base(tasks)
    {
        ProjectName = projectName;
    }
}

public sealed class CloudServiceSetting
{
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}

public sealed class LocalAgentSetting
{
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = "http://127.0.0.1:11434";
    public bool Enabled { get; set; }
}

public class ProjectPortfolioController
{
    private readonly ProjectDataService _projectDataService;
    private readonly SettingsDataService _settingsDataService;

    public ProjectPortfolioController(ProjectDataService projectDataService, SettingsDataService settingsDataService)
    {
        _projectDataService = projectDataService;
        _settingsDataService = settingsDataService;
    }

    public IReadOnlyList<ProjectSummary> GetProjectsForUser()
    {
        return _projectDataService.GetProjectSummariesAsync().GetAwaiter().GetResult();
    }

    public IReadOnlyList<ProjectActivity> GetActivitiesForProject(string? projectName)
    {
        return _projectDataService.GetActivitiesAsync(projectName).GetAwaiter().GetResult();
    }

    public IReadOnlyList<ProjectTask> GetProjectTasks()
    {
        return _projectDataService.GetTasksAsync().GetAwaiter().GetResult();
    }

    public IReadOnlyList<CloudServiceSetting> GetCloudServices()
    {
        return _settingsDataService.GetCloudServicesAsync().GetAwaiter().GetResult();
    }

    public IReadOnlyList<LocalAgentSetting> GetLocalAgents()
    {
        return _settingsDataService.GetLocalAgentsAsync().GetAwaiter().GetResult();
    }
}
