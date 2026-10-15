using FenixEngine.AppCore.Src.Models;
using FenixEngine.Services.Src.ProjectContext;
using FenixEngine.Shared.Src.Models;
using FenixEngine.Shared.Src.Repositories;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.AppCore.Src.Services;

public sealed class DevelopmentProjectService
{
    private readonly IDevelopmentProjectRepository _projectRepository;
    private readonly IProjectArtifactRepository _artifactRepository;
    private readonly IAgentTeamRepository _teamRepository;
    private readonly IProjectContextBuilder _contextBuilder;

    public DevelopmentProjectService(
        IDevelopmentProjectRepository projectRepository,
        IProjectArtifactRepository artifactRepository,
        IAgentTeamRepository teamRepository,
        IProjectContextBuilder contextBuilder)
    {
        _projectRepository = projectRepository;
        _artifactRepository = artifactRepository;
        _teamRepository = teamRepository;
        _contextBuilder = contextBuilder;
    }

    public async Task<IReadOnlyList<DevelopmentProjectListItemDto>> GetProjectsAsync(
        string? searchText = null,
        ProjectStatus? status = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var projects = await _projectRepository.GetAllAsync(searchText, status, isActive, cancellationToken);
        return projects.Select(project => new DevelopmentProjectListItemDto(
            project.Id,
            project.Name,
            project.Description,
            project.Status,
            project.IsActive,
            project.UpdatedAt,
            project.TeamAssignments.Count,
            project.Artifacts.Count)).ToArray();
    }

    public async Task<DevelopmentProjectDetailsDto> GetProjectAsync(string id, CancellationToken cancellationToken = default)
    {
        var project = await RequireProjectAsync(id, cancellationToken);
        return ToDetails(project);
    }

    public async Task<DevelopmentProjectDetailsDto> SaveProjectAsync(
        DevelopmentProjectInput input,
        CancellationToken cancellationToken = default)
    {
        ValidateProject(input);
        var teamIds = input.TeamIds.Distinct(StringComparer.Ordinal).ToArray();
        if (teamIds.Length == 0)
            throw new ArgumentException("Asigna al menos un equipo IA al proyecto.");
        if (!await _projectRepository.TeamsExistAsync(teamIds, cancellationToken))
            throw new InvalidOperationException("Uno o más equipos seleccionados ya no existen.");

        var isNew = string.IsNullOrWhiteSpace(input.Id);
        var id = isNew ? Guid.NewGuid().ToString() : input.Id!;
        var name = input.Name.Trim();
        if (await _projectRepository.ExistsByNameAsync(name, isNew ? null : id, cancellationToken))
            throw new InvalidOperationException($"Ya existe un proyecto llamado '{name}'.");

        var now = DateTime.UtcNow;
        var project = isNew
            ? new DevelopmentProject { Id = id, Name = name, Description = input.Description.Trim(), CreatedAt = now }
            : await RequireProjectAsync(id, cancellationToken);

        project.Name = name;
        project.Description = input.Description.Trim();
        project.BusinessGoal = input.BusinessGoal.Trim();
        project.FunctionalRequirements = input.FunctionalRequirements.Trim();
        project.NonFunctionalRequirements = input.NonFunctionalRequirements.Trim();
        project.ArchitectureStyle = input.ArchitectureStyle.Trim();
        project.TechnologyStack = input.TechnologyStack.Trim();
        project.Status = input.Status;
        project.IsActive = input.IsActive;
        project.UpdatedAt = now;

        if (isNew)
            await _projectRepository.AddAsync(project, teamIds, cancellationToken);
        else
            await _projectRepository.UpdateAsync(project, teamIds, cancellationToken);

        return await GetProjectAsync(project.Id, cancellationToken);
    }

    public Task DeleteProjectAsync(string id, CancellationToken cancellationToken = default) =>
        _projectRepository.DeleteAsync(id, cancellationToken);

    public async Task AssignTeamsAsync(string projectId, IEnumerable<string> teamIds, CancellationToken cancellationToken = default)
    {
        var ids = teamIds.Distinct(StringComparer.Ordinal).ToArray();
        if (ids.Length == 0)
            throw new ArgumentException("Asigna al menos un equipo IA al proyecto.");
        if (!await _projectRepository.TeamsExistAsync(ids, cancellationToken))
            throw new InvalidOperationException("Uno o más equipos seleccionados ya no existen.");

        await _projectRepository.AssignTeamsAsync(projectId, ids, cancellationToken);
    }

    public async Task<IReadOnlyList<DevelopmentProjectTeamDto>> GetAvailableTeamsAsync(
        string? projectId = null,
        CancellationToken cancellationToken = default)
    {
        var assignedIds = projectId is null
            ? new HashSet<string>(StringComparer.Ordinal)
            : (await RequireProjectAsync(projectId, cancellationToken))
                .TeamAssignments.Select(assignment => assignment.TeamId).ToHashSet(StringComparer.Ordinal);

        return (await _teamRepository.GetAllAsync(cancellationToken))
            .Select(team => new DevelopmentProjectTeamDto(
                team.Id,
                team.Name,
                team.Description,
                team.Members.Where(member => member.Agent is not null).Select(member => member.Agent!.Name).ToArray(),
                team.Members.Where(member => member.Agent?.Model is not null)
                    .Select(member => $"{member.Agent!.Model!.Name} ({member.Agent.Model.Provider?.Name ?? "Proveedor"})").ToArray(),
                assignedIds.Contains(team.Id)))
            .ToArray();
    }

    public async Task<ProjectArtifactDto> CreateArtifactAsync(ProjectArtifactInput input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.ProjectId))
            throw new ArgumentException("El proyecto es obligatorio.");
        if (string.IsNullOrWhiteSpace(input.Title))
            throw new ArgumentException("El título del artefacto es obligatorio.");
        if (!Enum.IsDefined(input.ArtifactType))
            throw new ArgumentOutOfRangeException(nameof(input.ArtifactType), "El tipo de artefacto no es válido.");

        await RequireProjectAsync(input.ProjectId, cancellationToken);
        var artifact = new ProjectArtifact
        {
            ProjectId = input.ProjectId,
            ArtifactType = input.ArtifactType,
            Title = input.Title.Trim(),
            Description = input.Description.Trim(),
            FilePath = input.FilePath.Trim()
        };
        await _artifactRepository.AddAsync(artifact, cancellationToken);
        return ToDto(artifact);
    }

    public async Task<IReadOnlyList<ProjectArtifactDto>> GetArtifactsAsync(string projectId, CancellationToken cancellationToken = default)
    {
        await RequireProjectAsync(projectId, cancellationToken);
        return (await _artifactRepository.GetByProjectAsync(projectId, cancellationToken)).Select(ToDto).ToArray();
    }

    public async Task<DevelopmentProjectContextDto> BuildProjectContextAsync(string projectId, CancellationToken cancellationToken = default)
    {
        var project = await RequireProjectAsync(projectId, cancellationToken);
        var context = _contextBuilder.Build(project);
        return new DevelopmentProjectContextDto(
            context.ProjectId,
            context.Name,
            context.Description,
            context.BusinessGoal,
            context.FunctionalRequirements,
            context.NonFunctionalRequirements,
            context.ArchitectureStyle,
            context.TechnologyStack,
            context.Status.ToString(),
            context.Teams.Select(team => new ProjectContextTeamDto(
                team.Id,
                team.Name,
                team.Agents.Select(agent => new ProjectContextAgentDto(
                    agent.Id,
                    agent.Name,
                    agent.Role,
                    agent.Description,
                    agent.SystemPrompt,
                    agent.ModelId,
                    agent.ModelName,
                    agent.ModelIdentifier,
                    agent.ProviderId,
                    agent.ProviderName,
                    agent.ProviderType,
                    agent.BaseUrl,
                    agent.IsLocal)).ToArray())).ToArray());
    }

    private async Task<DevelopmentProject> RequireProjectAsync(string id, CancellationToken cancellationToken) =>
        await _projectRepository.GetByIdAsync(id, cancellationToken)
        ?? throw new KeyNotFoundException("El proyecto seleccionado no existe.");

    private static void ValidateProject(DevelopmentProjectInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("El nombre del proyecto es obligatorio.");
        if (string.IsNullOrWhiteSpace(input.Description))
            throw new ArgumentException("La descripción del proyecto es obligatoria.");
        if (!Enum.IsDefined(input.Status))
            throw new ArgumentOutOfRangeException(nameof(input.Status), "El estado del proyecto no es válido.");
    }

    private static DevelopmentProjectDetailsDto ToDetails(DevelopmentProject project)
    {
        var teams = project.TeamAssignments
            .Where(assignment => assignment.Team is not null)
            .Select(assignment => new DevelopmentProjectTeamDto(
                assignment.Team!.Id,
                assignment.Team.Name,
                assignment.Team.Description,
                assignment.Team.Members.Where(member => member.Agent is not null).Select(member => member.Agent!.Name).ToArray(),
                assignment.Team.Members.Where(member => member.Agent?.Model is not null)
                    .Select(member => $"{member.Agent!.Model!.Name} ({member.Agent.Model.Provider?.Name ?? "Proveedor"})").ToArray(),
                true))
            .ToArray();

        return new DevelopmentProjectDetailsDto(
            project.Id,
            project.Name,
            project.Description,
            project.BusinessGoal,
            project.FunctionalRequirements,
            project.NonFunctionalRequirements,
            project.ArchitectureStyle,
            project.TechnologyStack,
            project.Status,
            project.IsActive,
            project.CreatedAt,
            project.UpdatedAt,
            teams,
            project.Artifacts.Select(ToDto).ToArray());
    }

    private static ProjectArtifactDto ToDto(ProjectArtifact artifact) =>
        new(artifact.Id, artifact.ProjectId, artifact.ArtifactType, artifact.Title,
            artifact.Description, artifact.FilePath, artifact.CreatedAt);
}