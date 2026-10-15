using FenixEngine.Shared.Src.Models;

namespace FenixEngine.AppCore.Src.Models;

public sealed record DevelopmentProjectListItemDto(
    string Id,
    string Name,
    string Description,
    ProjectStatus Status,
    bool IsActive,
    DateTime UpdatedAt,
    int TeamCount,
    int ArtifactCount);

public sealed record DevelopmentProjectTeamDto(
    string Id,
    string Name,
    string Description,
    IReadOnlyList<string> AgentNames,
    IReadOnlyList<string> AgentModelNames,
    bool IsAssigned);

public sealed record DevelopmentProjectDetailsDto(
    string Id,
    string Name,
    string Description,
    string BusinessGoal,
    string FunctionalRequirements,
    string NonFunctionalRequirements,
    string ArchitectureStyle,
    string TechnologyStack,
    ProjectStatus Status,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<DevelopmentProjectTeamDto> Teams,
    IReadOnlyList<ProjectArtifactDto> Artifacts);

public sealed record DevelopmentProjectInput(
    string? Id,
    string Name,
    string Description,
    string BusinessGoal,
    string FunctionalRequirements,
    string NonFunctionalRequirements,
    string ArchitectureStyle,
    string TechnologyStack,
    ProjectStatus Status,
    bool IsActive,
    IReadOnlyList<string> TeamIds);

public sealed record ProjectArtifactDto(
    string Id,
    string ProjectId,
    ArtifactType ArtifactType,
    string Title,
    string Description,
    string FilePath,
    DateTime CreatedAt);

public sealed record ProjectArtifactInput(
    string ProjectId,
    ArtifactType ArtifactType,
    string Title,
    string Description,
    string FilePath);

public sealed record DevelopmentProjectContextDto(
    string ProjectId,
    string Name,
    string Description,
    string BusinessGoal,
    string FunctionalRequirements,
    string NonFunctionalRequirements,
    string ArchitectureStyle,
    string TechnologyStack,
    string Status,
    IReadOnlyList<ProjectContextTeamDto> Teams);

public sealed record ProjectContextTeamDto(
    string TeamId,
    string TeamName,
    IReadOnlyList<ProjectContextAgentDto> Agents);

public sealed record ProjectContextAgentDto(
    string AgentId,
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