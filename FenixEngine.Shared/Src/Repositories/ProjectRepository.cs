using FenixEngine.Shared.Src.Models;
using FenixEngine.Shared.Src.Services.DataBase;
using Microsoft.EntityFrameworkCore;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Shared.Src.Repositories;

public interface IProjectRepository
{
    Task<List<GeneralProyects>> GetProjectsAsync(CancellationToken cancellationToken = default);
    Task<List<UserActivity>> GetActivitiesAsync(string? projectName = null, CancellationToken cancellationToken = default);
}

public interface IActivityRepository
{
    Task<List<UserActivity>> GetActivitiesAsync(string? projectName = null, CancellationToken cancellationToken = default);
    Task<List<UserActivity>> GetProjectTasksAsync(CancellationToken cancellationToken = default);
}

public interface ITaskRepository
{
    Task<List<ProjectTask>> GetTasksAsync(CancellationToken cancellationToken = default);
    Task<ProjectTask?> GetByIdAsync(string taskId, CancellationToken cancellationToken = default);
    Task AddAsync(ProjectTask task, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProjectTask task, CancellationToken cancellationToken = default);
}

public interface IUserRepository
{
    Task<UserArchitect?> AuthenticateAsync(string email, string passwordHash, CancellationToken cancellationToken = default);
}

public interface ISettingsRepository
{
    Task<List<AiApiToken>> GetCloudServicesAsync(CancellationToken cancellationToken = default);
    Task<List<LocalAgentConfiguration>> GetLocalAgentsAsync(CancellationToken cancellationToken = default);
    Task UpdateCloudServiceAsync(AiApiToken service, CancellationToken cancellationToken = default);
    Task UpdateLocalAgentAsync(LocalAgentConfiguration agent, CancellationToken cancellationToken = default);
}

public sealed class ProjectRepository : IProjectRepository
{
    private readonly ServiceDbContext _dbContext;

    public ProjectRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GeneralProyects>> GetProjectsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.GeneralProyects
            .AsNoTracking()
            .Include(p => p.ProyectType)
            .OrderByDescending(p => p.GProyectEdit ?? p.GProyectCreate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<UserActivity>> GetActivitiesAsync(string? projectName = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.UserActivities
            .AsNoTracking()
            .Include(a => a.Proyect)
            .Include(a => a.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(projectName) && !projectName.Equals("Todos", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(a => a.Proyect != null && a.Proyect.GProyectName == projectName);
        }

        return await query
            .OrderByDescending(a => a.UActivityCreation)
            .ToListAsync(cancellationToken);
    }
}

public sealed class ActivityRepository : IActivityRepository
{
    private readonly ServiceDbContext _dbContext;

    public ActivityRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserActivity>> GetActivitiesAsync(string? projectName = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.UserActivities
            .AsNoTracking()
            .Include(a => a.Proyect)
            .Include(a => a.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(projectName) && !projectName.Equals("Todos", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(a => a.Proyect != null && a.Proyect.GProyectName == projectName);
        }

        return await query
            .OrderByDescending(a => a.UActivityCreation)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<UserActivity>> GetProjectTasksAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserActivities
            .AsNoTracking()
            .Include(a => a.Proyect)
            .Where(a => a.Proyect != null)
            .OrderByDescending(a => a.UActivityCreation)
            .ToListAsync(cancellationToken);
    }
}

public sealed class TaskRepository : ITaskRepository
{
    private readonly ServiceDbContext _dbContext;

    public TaskRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProjectTask>> GetTasksAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProjectTasks
            .AsNoTracking()
            .Include(task => task.Project)
            .OrderBy(task => task.State == "Completed")
            .ThenByDescending(task => task.Priority)
            .ThenByDescending(task => task.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<ProjectTask?> GetByIdAsync(string taskId, CancellationToken cancellationToken = default)
    {
        return _dbContext.ProjectTasks
            .Include(task => task.Project)
            .FirstOrDefaultAsync(task => task.TaskId == taskId, cancellationToken);
    }

    public async Task AddAsync(ProjectTask task, CancellationToken cancellationToken = default)
    {
        _dbContext.ProjectTasks.Add(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProjectTask task, CancellationToken cancellationToken = default)
    {
        _dbContext.ProjectTasks.Update(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public sealed class UserRepository : IUserRepository
{
    private readonly ServiceDbContext _dbContext;

    public UserRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UserArchitect?> AuthenticateAsync(string email, string passwordHash, CancellationToken cancellationToken = default)
    {
        return _dbContext.UserArchitects
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email && user.PasswordHash == passwordHash, cancellationToken);
    }
}

public sealed class SettingsRepository : ISettingsRepository
{
    private readonly ServiceDbContext _dbContext;

    public SettingsRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<AiApiToken>> GetCloudServicesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.AiApiTokens
            .AsNoTracking()
            .OrderBy(service => service.ProjectName)
            .ToListAsync(cancellationToken);
    }

    public Task<List<LocalAgentConfiguration>> GetLocalAgentsAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.LocalAgentConfigurations
            .AsNoTracking()
            .OrderBy(agent => agent.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateCloudServiceAsync(AiApiToken service, CancellationToken cancellationToken = default)
    {
        _dbContext.AiApiTokens.Update(service);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateLocalAgentAsync(LocalAgentConfiguration agent, CancellationToken cancellationToken = default)
    {
        _dbContext.LocalAgentConfigurations.Update(agent);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
