namespace FenixEngine.AppCore.Src.Models;

public sealed record AiProviderDto(
    string Id,
    string Name,
    string ProviderType,
    string BaseUrl,
    bool IsLocal,
    bool IsEnabled,
    bool ApiKeyConfigured);

public sealed record AiProviderInput(
    string? Id,
    string Name,
    string ProviderType,
    string BaseUrl,
    bool IsLocal,
    bool IsEnabled,
    string? ApiKey,
    bool ClearApiKey = false);

public sealed record LlmModelDto(
    string Id,
    string ProviderId,
    string ProviderName,
    string Name,
    string ModelIdentifier,
    int? ContextWindow,
    bool IsEnabled);

public sealed record LlmModelInput(
    string? Id,
    string ProviderId,
    string Name,
    string ModelIdentifier,
    int? ContextWindow,
    bool IsEnabled);

public sealed record AiAgentDto(
    string Id,
    string Name,
    string Description,
    string Role,
    string ModelId,
    string ModelName,
    string ProviderName,
    string SystemPrompt,
    bool IsEnabled);

public sealed record AiAgentInput(
    string? Id,
    string Name,
    string Description,
    string Role,
    string ModelId,
    string SystemPrompt,
    bool IsEnabled);

public sealed record AgentTeamDto(
    string Id,
    string Name,
    string Description,
    IReadOnlyList<string> AgentIds,
    IReadOnlyList<string> AgentNames);

public sealed record AgentTeamInput(
    string? Id,
    string Name,
    string Description,
    IReadOnlyList<string> AgentIds);
