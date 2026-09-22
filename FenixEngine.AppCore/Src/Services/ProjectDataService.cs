using FenixEngine.AppCore.Controllers;
using FenixEngine.Shared.Src.Models;
using FenixEngine.Shared.Src.Repositories;
using ProjectTaskDto = FenixEngine.AppCore.Controllers.ProjectTask;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.AppCore.Src.Services;

public sealed class ProjectDataService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly ITaskRepository _taskRepository;

    public ProjectDataService(
        IProjectRepository projectRepository,
        IActivityRepository activityRepository,
        ITaskRepository taskRepository)
    {
        _projectRepository = projectRepository;
        _activityRepository = activityRepository;
        _taskRepository = taskRepository;
    }

    public async Task<List<ProjectSummary>> GetProjectSummariesAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _projectRepository.GetProjectsAsync(cancellationToken);

        return projects.Select(p => new ProjectSummary
        {
            Name = p.GProyectName,
            Type = p.ProyectType?.PTypeName ?? "Local",
            LastModified = p.GProyectEdit ?? p.GProyectCreate,
            Status = string.IsNullOrWhiteSpace(p.GProyectComment) ? "Active" : "Review"
        }).ToList();
    }

    public async Task<List<ProjectActivity>> GetActivitiesAsync(string? projectName = null, CancellationToken cancellationToken = default)
    {
        var activities = await _activityRepository.GetActivitiesAsync(projectName, cancellationToken);

        return activities.Select(a => new ProjectActivity
        {
            Title = a.Proyect != null ? $"Actividad en {a.Proyect.GProyectName}" : "Actividad del usuario",
            ProjectName = a.Proyect?.GProyectName ?? projectName ?? "Todos",
            CreatedAt = a.UActivityCreation,
            Type = "Update"
        }).ToList();
    }

    public async Task<List<ProjectTaskDto>> GetTasksAsync(CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetTasksAsync(cancellationToken);

        return tasks.Select(task => new ProjectTaskDto
        {
            ProjectName = task.Project?.GProyectName ?? "Sin proyecto",
            Title = task.Title,
            State = task.State,
            CreatedAt = task.CreatedAt
        }).ToList();
    }
}
