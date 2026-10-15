using FenixEngine.Shared.Src.Models;
using FenixEngine.Shared.Src.Services.DataBase;
using Microsoft.EntityFrameworkCore;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Shared.Src.Repositories;

public interface IDevelopmentProjectRepository
{
    Task<List<DevelopmentProject>> GetAllAsync(
        string? searchText = null,
        ProjectStatus? status = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);
    Task<DevelopmentProject?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, string? excludingId = null, CancellationToken cancellationToken = default);
    Task<bool> TeamsExistAsync(IEnumerable<string> teamIds, CancellationToken cancellationToken = default);
    Task AddAsync(DevelopmentProject project, IEnumerable<string> teamIds, CancellationToken cancellationToken = default);
    Task UpdateAsync(DevelopmentProject project, IEnumerable<string> teamIds, CancellationToken cancellationToken = default);
    Task AssignTeamsAsync(string projectId, IEnumerable<string> teamIds, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IProjectArtifactRepository
{
    Task<List<ProjectArtifact>> GetByProjectAsync(string projectId, CancellationToken cancellationToken = default);
    Task AddAsync(ProjectArtifact artifact, CancellationToken cancellationToken = default);
}

public sealed class DevelopmentProjectRepository : IDevelopmentProjectRepository
{
    private readonly ServiceDbContext _dbContext;

    public DevelopmentProjectRepository(ServiceDbContext dbContext) => _dbContext = dbContext;

    public async Task<List<DevelopmentProject>> GetAllAsync(
        string? searchText = null,
        ProjectStatus? status = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.DevelopmentProjects.AsNoTracking()
            .Include(project => project.TeamAssignments)
            .Include(project => project.Artifacts)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var search = searchText.Trim();
            query = query.Where(project => EF.Functions.Like(project.Name, $"%{search}%"));
        }

        if (status.HasValue)
            query = query.Where(project => project.Status == status.Value);

        if (isActive.HasValue)
            query = query.Where(project => project.IsActive == isActive.Value);

        return await query.OrderByDescending(project => project.UpdatedAt).ToListAsync(cancellationToken);
    }

    public Task<DevelopmentProject?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        _dbContext.DevelopmentProjects.AsNoTracking()
            .Include(project => project.TeamAssignments)
                .ThenInclude(assignment => assignment.Team)
                    .ThenInclude(team => team!.Members)
                        .ThenInclude(member => member.Agent)
                            .ThenInclude(agent => agent!.Model)
                                .ThenInclude(model => model!.Provider)
            .Include(project => project.Artifacts)
            .FirstOrDefaultAsync(project => project.Id == id, cancellationToken);

    public Task<bool> ExistsByNameAsync(string name, string? excludingId = null, CancellationToken cancellationToken = default) =>
        _dbContext.DevelopmentProjects.AnyAsync(
            project => project.Name == name && (excludingId == null || project.Id != excludingId),
            cancellationToken);

    public async Task<bool> TeamsExistAsync(IEnumerable<string> teamIds, CancellationToken cancellationToken = default)
    {
        var ids = teamIds.Distinct(StringComparer.Ordinal).ToArray();
        if (ids.Length == 0) return true;

        return await _dbContext.AgentTeams.CountAsync(team => ids.Contains(team.Id), cancellationToken) == ids.Length;
    }

    public async Task AddAsync(DevelopmentProject project, IEnumerable<string> teamIds, CancellationToken cancellationToken = default)
    {
        project.TeamAssignments = CreateAssignments(project.Id, teamIds);
        _dbContext.DevelopmentProjects.Add(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(DevelopmentProject project, IEnumerable<string> teamIds, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.DevelopmentProjects
            .Include(item => item.TeamAssignments)
            .FirstOrDefaultAsync(item => item.Id == project.Id, cancellationToken)
            ?? throw new KeyNotFoundException("El proyecto seleccionado ya no existe.");

        existing.Name = project.Name;
        existing.Description = project.Description;
        existing.BusinessGoal = project.BusinessGoal;
        existing.FunctionalRequirements = project.FunctionalRequirements;
        existing.NonFunctionalRequirements = project.NonFunctionalRequirements;
        existing.ArchitectureStyle = project.ArchitectureStyle;
        existing.TechnologyStack = project.TechnologyStack;
        existing.Status = project.Status;
        existing.IsActive = project.IsActive;
        existing.UpdatedAt = project.UpdatedAt;
        ReplaceAssignments(existing, teamIds);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AssignTeamsAsync(string projectId, IEnumerable<string> teamIds, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.DevelopmentProjects
            .Include(item => item.TeamAssignments)
            .FirstOrDefaultAsync(item => item.Id == projectId, cancellationToken)
            ?? throw new KeyNotFoundException("El proyecto seleccionado ya no existe.");

        ReplaceAssignments(project, teamIds);
        project.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.DevelopmentProjects.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (project is null) return;

        _dbContext.DevelopmentProjects.Remove(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static List<DevelopmentProjectTeam> CreateAssignments(string projectId, IEnumerable<string> teamIds) =>
        teamIds.Distinct(StringComparer.Ordinal)
            .Select(teamId => new DevelopmentProjectTeam { ProjectId = projectId, TeamId = teamId })
            .ToList();

    private void ReplaceAssignments(DevelopmentProject project, IEnumerable<string> teamIds)
    {
        var desiredTeamIds = teamIds.Distinct(StringComparer.Ordinal).ToHashSet(StringComparer.Ordinal);
        var assignmentsToRemove = project.TeamAssignments
            .Where(assignment => !desiredTeamIds.Contains(assignment.TeamId))
            .ToArray();
        _dbContext.DevelopmentProjectTeams.RemoveRange(assignmentsToRemove);

        var existingTeamIds = project.TeamAssignments.Select(assignment => assignment.TeamId).ToHashSet(StringComparer.Ordinal);
        foreach (var teamId in desiredTeamIds.Where(teamId => !existingTeamIds.Contains(teamId)))
        {
            project.TeamAssignments.Add(new DevelopmentProjectTeam { ProjectId = project.Id, TeamId = teamId });
        }
    }
}

public sealed class ProjectArtifactRepository : IProjectArtifactRepository
{
    private readonly ServiceDbContext _dbContext;

    public ProjectArtifactRepository(ServiceDbContext dbContext) => _dbContext = dbContext;

    public Task<List<ProjectArtifact>> GetByProjectAsync(string projectId, CancellationToken cancellationToken = default) =>
        _dbContext.ProjectArtifacts.AsNoTracking()
            .Where(artifact => artifact.ProjectId == projectId)
            .OrderByDescending(artifact => artifact.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(ProjectArtifact artifact, CancellationToken cancellationToken = default)
    {
        _dbContext.ProjectArtifacts.Add(artifact);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}