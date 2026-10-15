using FenixEngine.Shared.Src.Models;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Services.Src.ProjectContext;

public sealed record ProjectContextAgent(
    string Id,
    string Name,
    string Role,
    string Description,
    string SystemPrompt,
    string ModelId,
    string ModelName,
    string ModelIdentifier,
    string ProviderId,
    string ProviderName,
    string ProviderType,
    string BaseUrl,
    bool IsLocal);

public sealed record ProjectContextTeam(string Id, string Name, IReadOnlyList<ProjectContextAgent> Agents);

public sealed record ProjectContext(
    string ProjectId,
    string Name,
    string Description,
    string BusinessGoal,
    string FunctionalRequirements,
    string NonFunctionalRequirements,
    string ArchitectureStyle,
    string TechnologyStack,
    ProjectStatus Status,
    IReadOnlyList<ProjectContextTeam> Teams);

public interface IProjectContextBuilder
{
    ProjectContext Build(DevelopmentProject project);
}

public sealed class ProjectContextBuilder : IProjectContextBuilder
{
    public ProjectContext Build(DevelopmentProject project)
    {
        var teams = project.TeamAssignments
            .Where(assignment => assignment.Team is not null)
            .Select(assignment => new ProjectContextTeam(
                assignment.Team!.Id,
                assignment.Team.Name,
                assignment.Team.Members
                    .Where(member => member.Agent?.Model?.Provider is not null)
                    .Select(member =>
                    {
                        var agent = member.Agent!;
                        var model = agent.Model!;
                        var provider = model.Provider!;
                        return new ProjectContextAgent(
                            agent.Id,
                            agent.Name,
                            agent.Role,
                            agent.Description,
                            agent.SystemPrompt,
                            model.Id,
                            model.Name,
                            model.ModelIdentifier,
                            provider.Id,
                            provider.Name,
                            provider.ProviderType,
                            provider.BaseUrl,
                            provider.IsLocal);
                    })
                    .ToArray()))
            .ToArray();

        return new ProjectContext(
            project.Id,
            project.Name,
            project.Description,
            project.BusinessGoal,
            project.FunctionalRequirements,
            project.NonFunctionalRequirements,
            project.ArchitectureStyle,
            project.TechnologyStack,
            project.Status,
            teams);
    }
}