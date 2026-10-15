using FenixEngine.AppCore.Src.Models;
using FenixEngine.Services.Src.Providers;
using FenixEngine.Shared.Src.Models;
using FenixEngine.Shared.Src.Repositories;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.AppCore.Src.Services;

public sealed class AiEcosystemService
{
    private readonly IAiProviderRepository _providerRepository;
    private readonly ILlmModelRepository _modelRepository;
    private readonly IAiAgentRepository _agentRepository;
    private readonly IAgentTeamRepository _teamRepository;
    private readonly IAProviderConnectorFactory _connectorFactory;
    private readonly IApiKeyStore _apiKeyStore;

    public AiEcosystemService(
        IAiProviderRepository providerRepository,
        ILlmModelRepository modelRepository,
        IAiAgentRepository agentRepository,
        IAgentTeamRepository teamRepository,
        IAProviderConnectorFactory connectorFactory,
        IApiKeyStore apiKeyStore)
    {
        _providerRepository = providerRepository;
        _modelRepository = modelRepository;
        _agentRepository = agentRepository;
        _teamRepository = teamRepository;
        _connectorFactory = connectorFactory;
        _apiKeyStore = apiKeyStore;
    }

    public async Task<IReadOnlyList<AiProviderDto>> GetProvidersAsync(CancellationToken cancellationToken = default)
    {
        var providers = await _providerRepository.GetAllAsync(cancellationToken);
        var result = new List<AiProviderDto>(providers.Count);
        foreach (var provider in providers)
        {
            result.Add(ToDto(provider, !string.IsNullOrEmpty(await _apiKeyStore.GetAsync(provider.Id))));
        }

        return result;
    }

    public async Task<AiProviderDto> SaveProviderAsync(AiProviderInput input, CancellationToken cancellationToken = default)
    {
        ValidateProvider(input);
        _connectorFactory.GetConnector(input.ProviderType);

        var isNew = string.IsNullOrWhiteSpace(input.Id);
        var provider = isNew
            ? new AiProvider { Name = input.Name.Trim(), ProviderType = input.ProviderType, BaseUrl = input.BaseUrl.Trim(), IsLocal = input.IsLocal, IsEnabled = input.IsEnabled }
            : await _providerRepository.GetByIdAsync(input.Id!, cancellationToken)
                ?? throw new KeyNotFoundException("El proveedor seleccionado ya no existe.");

        if (!isNew)
        {
            provider.Name = input.Name.Trim();
            provider.ProviderType = input.ProviderType;
            provider.BaseUrl = input.BaseUrl.Trim();
            provider.IsLocal = input.IsLocal;
            provider.IsEnabled = input.IsEnabled;
            provider.UpdatedAt = DateTime.UtcNow;
        }

        if (isNew)
        {
            await _providerRepository.AddAsync(provider, cancellationToken);
        }
        else
        {
            await _providerRepository.UpdateAsync(provider, cancellationToken);
        }

        if (input.ClearApiKey)
            await _apiKeyStore.RemoveAsync(provider.Id);
        else if (!string.IsNullOrWhiteSpace(input.ApiKey))
            await _apiKeyStore.SetAsync(provider.Id, input.ApiKey.Trim());

        return ToDto(provider, !string.IsNullOrEmpty(await _apiKeyStore.GetAsync(provider.Id)));
    }

    public async Task DeleteProviderAsync(string id, CancellationToken cancellationToken = default)
    {
        if ((await _modelRepository.GetByProviderAsync(id, cancellationToken)).Count > 0)
            throw new InvalidOperationException("Elimina primero los modelos asociados a este proveedor.");

        await _providerRepository.DeleteAsync(id, cancellationToken);
        await _apiKeyStore.RemoveAsync(id);
    }

    public async Task TestProviderAsync(string id, CancellationToken cancellationToken = default)
    {
        var provider = await RequireProviderAsync(id, cancellationToken);
        var apiKey = await _apiKeyStore.GetAsync(id);
        if (!provider.IsLocal && string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("Configura una API key antes de probar este proveedor.");

        await _connectorFactory.GetConnector(provider.ProviderType).TestConnectionAsync(provider, apiKey, cancellationToken);
    }

    public async Task<int> SynchronizeModelsAsync(string providerId, CancellationToken cancellationToken = default)
    {
        var provider = await RequireProviderAsync(providerId, cancellationToken);
        var apiKey = await _apiKeyStore.GetAsync(providerId);
        if (!provider.IsLocal && string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("Configura una API key antes de sincronizar modelos.");

        var discovered = await _connectorFactory.GetConnector(provider.ProviderType)
            .GetModelsAsync(provider, apiKey, cancellationToken);
        var existing = await _modelRepository.GetByProviderAsync(providerId, cancellationToken);
        var existingByIdentifier = existing.ToDictionary(model => model.ModelIdentifier, StringComparer.OrdinalIgnoreCase);

        foreach (var discoveredModel in discovered)
        {
            if (existingByIdentifier.TryGetValue(discoveredModel.ModelIdentifier, out var model))
            {
                model.Name = discoveredModel.Name;
                model.ContextWindow = discoveredModel.ContextWindow ?? model.ContextWindow;
                await _modelRepository.UpdateAsync(model, cancellationToken);
            }
            else
            {
                await _modelRepository.AddAsync(new LlmModel
                {
                    ProviderId = providerId,
                    Name = discoveredModel.Name,
                    ModelIdentifier = discoveredModel.ModelIdentifier,
                    ContextWindow = discoveredModel.ContextWindow,
                    IsEnabled = true
                }, cancellationToken);
            }
        }

        return discovered.Count;
    }

    public async Task<IReadOnlyList<LlmModelDto>> GetModelsAsync(CancellationToken cancellationToken = default) =>
        (await _modelRepository.GetAllAsync(cancellationToken)).Select(ToDto).ToArray();

    public async Task<LlmModelDto> SaveModelAsync(LlmModelInput input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.ProviderId) || string.IsNullOrWhiteSpace(input.Name) || string.IsNullOrWhiteSpace(input.ModelIdentifier))
            throw new ArgumentException("Proveedor, nombre e identificador del modelo son obligatorios.");
        if (input.ContextWindow is <= 0)
            throw new ArgumentOutOfRangeException(nameof(input.ContextWindow), "La ventana de contexto debe ser mayor que cero.");
        if (await _providerRepository.GetByIdAsync(input.ProviderId, cancellationToken) is null)
            throw new InvalidOperationException("El proveedor seleccionado ya no existe.");

        var model = string.IsNullOrWhiteSpace(input.Id)
            ? new LlmModel { ProviderId = input.ProviderId, Name = input.Name.Trim(), ModelIdentifier = input.ModelIdentifier.Trim() }
            : await _modelRepository.GetByIdAsync(input.Id!, cancellationToken)
                ?? throw new KeyNotFoundException("El modelo seleccionado ya no existe.");

        model.ProviderId = input.ProviderId;
        model.Name = input.Name.Trim();
        model.ModelIdentifier = input.ModelIdentifier.Trim();
        model.ContextWindow = input.ContextWindow;
        model.IsEnabled = input.IsEnabled;

        if (string.IsNullOrWhiteSpace(input.Id))
            await _modelRepository.AddAsync(model, cancellationToken);
        else
            await _modelRepository.UpdateAsync(model, cancellationToken);

        return ToDto(model, await _providerRepository.GetByIdAsync(model.ProviderId, cancellationToken));
    }

    public async Task DeleteModelAsync(string id, CancellationToken cancellationToken = default)
    {
        if ((await _agentRepository.GetAllAsync(cancellationToken)).Any(agent => agent.ModelId == id))
            throw new InvalidOperationException("Reasigna o elimina primero los agentes que usan este modelo.");

        await _modelRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<LlmModelDto>> GetEnabledModelsAsync(CancellationToken cancellationToken = default) =>
        (await _modelRepository.GetAllAsync(cancellationToken))
            .Where(model => model.IsEnabled && model.Provider?.IsEnabled == true)
            .Select(ToDto).ToArray();

    public async Task<IReadOnlyList<AiAgentDto>> GetAgentsAsync(CancellationToken cancellationToken = default) =>
        (await _agentRepository.GetAllAsync(cancellationToken)).Select(ToDto).ToArray();

    public async Task<AiAgentDto> SaveAgentAsync(AiAgentInput input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.Name) || string.IsNullOrWhiteSpace(input.Role)
            || string.IsNullOrWhiteSpace(input.ModelId) || string.IsNullOrWhiteSpace(input.SystemPrompt))
            throw new ArgumentException("Nombre, rol, modelo y prompt de sistema son obligatorios.");

        var model = await _modelRepository.GetByIdAsync(input.ModelId, cancellationToken)
            ?? throw new InvalidOperationException("El modelo seleccionado ya no existe.");
        if (!model.IsEnabled || model.Provider?.IsEnabled != true)
            throw new InvalidOperationException("El agente debe utilizar un modelo y proveedor habilitados.");

        var isNew = string.IsNullOrWhiteSpace(input.Id);
        var agent = isNew
            ? new AiAgent { Name = input.Name.Trim(), Role = input.Role.Trim(), ModelId = input.ModelId, SystemPrompt = input.SystemPrompt.Trim() }
            : await _agentRepository.GetByIdAsync(input.Id!, cancellationToken)
                ?? throw new KeyNotFoundException("El agente seleccionado ya no existe.");

        agent.Name = input.Name.Trim();
        agent.Description = input.Description.Trim();
        agent.Role = input.Role.Trim();
        agent.ModelId = input.ModelId;
        agent.SystemPrompt = input.SystemPrompt.Trim();
        agent.IsEnabled = input.IsEnabled;

        if (isNew)
            await _agentRepository.AddAsync(agent, cancellationToken);
        else
            await _agentRepository.UpdateAsync(agent, cancellationToken);

        agent.Model = model;
        return ToDto(agent);
    }

    public Task DeleteAgentAsync(string id, CancellationToken cancellationToken = default) =>
        _agentRepository.DeleteAsync(id, cancellationToken);

    public async Task<IReadOnlyList<AgentTeamDto>> GetTeamsAsync(CancellationToken cancellationToken = default) =>
        (await _teamRepository.GetAllAsync(cancellationToken)).Select(ToDto).ToArray();

    public async Task<AgentTeamDto> SaveTeamAsync(AgentTeamInput input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("El nombre del equipo es obligatorio.");

        var isNew = string.IsNullOrWhiteSpace(input.Id);
        var team = isNew
            ? new AgentTeam { Name = input.Name.Trim(), Description = input.Description.Trim() }
            : await _teamRepository.GetByIdAsync(input.Id!, cancellationToken)
                ?? throw new KeyNotFoundException("El equipo seleccionado ya no existe.");

        team.Name = input.Name.Trim();
        team.Description = input.Description.Trim();
        if (isNew)
            await _teamRepository.AddAsync(team, input.AgentIds, cancellationToken);
        else
            await _teamRepository.UpdateAsync(team, input.AgentIds, cancellationToken);

        var saved = await _teamRepository.GetByIdAsync(team.Id, cancellationToken)
            ?? throw new InvalidOperationException("No fue posible recuperar el equipo guardado.");
        return ToDto(saved);
    }

    public Task DeleteTeamAsync(string id, CancellationToken cancellationToken = default) =>
        _teamRepository.DeleteAsync(id, cancellationToken);

    private async Task<AiProvider> RequireProviderAsync(string id, CancellationToken cancellationToken) =>
        await _providerRepository.GetByIdAsync(id, cancellationToken)
        ?? throw new KeyNotFoundException("El proveedor seleccionado ya no existe.");

    private static void ValidateProvider(AiProviderInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name) || string.IsNullOrWhiteSpace(input.ProviderType)
            || !Uri.TryCreate(input.BaseUrl, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            throw new ArgumentException("Nombre, tipo y una URL HTTP/HTTPS válida son obligatorios.");
    }

    private static AiProviderDto ToDto(AiProvider provider, bool apiKeyConfigured) =>
        new(provider.Id, provider.Name, provider.ProviderType, provider.BaseUrl, provider.IsLocal, provider.IsEnabled, apiKeyConfigured);

    private static LlmModelDto ToDto(LlmModel model) => ToDto(model, model.Provider);

    private static LlmModelDto ToDto(LlmModel model, AiProvider? provider) =>
        new(model.Id, model.ProviderId, provider?.Name ?? string.Empty, model.Name, model.ModelIdentifier, model.ContextWindow, model.IsEnabled);

    private static AiAgentDto ToDto(AiAgent agent) =>
        new(agent.Id, agent.Name, agent.Description, agent.Role, agent.ModelId,
            agent.Model?.Name ?? string.Empty, agent.Model?.Provider?.Name ?? string.Empty, agent.SystemPrompt, agent.IsEnabled);

    private static AgentTeamDto ToDto(AgentTeam team)
    {
        var agents = team.Members.Where(member => member.Agent is not null).Select(member => member.Agent!).ToArray();
        return new AgentTeamDto(team.Id, team.Name, team.Description,
            agents.Select(agent => agent.Id).ToArray(), agents.Select(agent => agent.Name).ToArray());
    }
}