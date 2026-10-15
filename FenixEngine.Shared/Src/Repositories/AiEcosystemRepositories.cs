using FenixEngine.Shared.Src.Models;
using FenixEngine.Shared.Src.Services.DataBase;
using Microsoft.EntityFrameworkCore;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Shared.Src.Repositories;

public sealed class AiProviderRepository : IAiProviderRepository
{
    private readonly ServiceDbContext _dbContext;

    public AiProviderRepository(ServiceDbContext dbContext) => _dbContext = dbContext;

    public Task<List<AiProvider>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _dbContext.AiProviders.AsNoTracking().OrderBy(provider => provider.Name).ToListAsync(cancellationToken);

    public Task<AiProvider?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        _dbContext.AiProviders.FirstOrDefaultAsync(provider => provider.Id == id, cancellationToken);

    public async Task AddAsync(AiProvider provider, CancellationToken cancellationToken = default)
    {
        _dbContext.AiProviders.Add(provider);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AiProvider provider, CancellationToken cancellationToken = default)
    {
        _dbContext.AiProviders.Update(provider);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var provider = await _dbContext.AiProviders.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (provider is null) return;

        _dbContext.AiProviders.Remove(provider);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public sealed class LlmModelRepository : ILlmModelRepository
{
    private readonly ServiceDbContext _dbContext;

    public LlmModelRepository(ServiceDbContext dbContext) => _dbContext = dbContext;

    public Task<List<LlmModel>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _dbContext.LlmModels.AsNoTracking().Include(model => model.Provider)
            .OrderBy(model => model.Provider!.Name).ThenBy(model => model.Name).ToListAsync(cancellationToken);

    public Task<List<LlmModel>> GetByProviderAsync(string providerId, CancellationToken cancellationToken = default) =>
        _dbContext.LlmModels.AsNoTracking().Where(model => model.ProviderId == providerId)
            .OrderBy(model => model.Name).ToListAsync(cancellationToken);

    public Task<LlmModel?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        _dbContext.LlmModels.Include(model => model.Provider)
            .FirstOrDefaultAsync(model => model.Id == id, cancellationToken);

    public async Task AddAsync(LlmModel model, CancellationToken cancellationToken = default)
    {
        _dbContext.LlmModels.Add(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(LlmModel model, CancellationToken cancellationToken = default)
    {
        _dbContext.LlmModels.Update(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var model = await _dbContext.LlmModels.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (model is null) return;

        _dbContext.LlmModels.Remove(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public sealed class AiAgentRepository : IAiAgentRepository
{
    private readonly ServiceDbContext _dbContext;

    public AiAgentRepository(ServiceDbContext dbContext) => _dbContext = dbContext;

    public Task<List<AiAgent>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _dbContext.AiAgents.AsNoTracking().Include(agent => agent.Model).ThenInclude(model => model!.Provider)
            .OrderBy(agent => agent.Name).ToListAsync(cancellationToken);

    public Task<AiAgent?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        _dbContext.AiAgents.Include(agent => agent.Model).ThenInclude(model => model!.Provider)
            .FirstOrDefaultAsync(agent => agent.Id == id, cancellationToken);

    public async Task AddAsync(AiAgent agent, CancellationToken cancellationToken = default)
    {
        _dbContext.AiAgents.Add(agent);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AiAgent agent, CancellationToken cancellationToken = default)
    {
        _dbContext.AiAgents.Update(agent);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var agent = await _dbContext.AiAgents.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (agent is null) return;

        _dbContext.AiAgents.Remove(agent);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public sealed class AgentTeamRepository : IAgentTeamRepository
{
    private readonly ServiceDbContext _dbContext;

    public AgentTeamRepository(ServiceDbContext dbContext) => _dbContext = dbContext;

    public Task<List<AgentTeam>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _dbContext.AgentTeams.AsNoTracking().Include(team => team.Members)
            .ThenInclude(member => member.Agent).ThenInclude(agent => agent!.Model)
            .OrderBy(team => team.Name).ToListAsync(cancellationToken);

    public Task<AgentTeam?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        _dbContext.AgentTeams.Include(team => team.Members).ThenInclude(member => member.Agent)
            .FirstOrDefaultAsync(team => team.Id == id, cancellationToken);

    public async Task AddAsync(AgentTeam team, IEnumerable<string> memberIds, CancellationToken cancellationToken = default)
    {
        var ids = memberIds.Distinct(StringComparer.Ordinal).ToArray();
        await EnsureAgentsExistAsync(ids, cancellationToken);
        team.Members = ids.Select(agentId => new AgentTeamMember { TeamId = team.Id, AgentId = agentId }).ToList();
        _dbContext.AgentTeams.Add(team);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AgentTeam team, IEnumerable<string> memberIds, CancellationToken cancellationToken = default)
    {
        var ids = memberIds.Distinct(StringComparer.Ordinal).ToArray();
        await EnsureAgentsExistAsync(ids, cancellationToken);

        var existing = await _dbContext.AgentTeams.Include(item => item.Members)
            .FirstOrDefaultAsync(item => item.Id == team.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"No existe el equipo '{team.Id}'.");

        existing.Name = team.Name;
        existing.Description = team.Description;
        _dbContext.AgentTeamMembers.RemoveRange(existing.Members);
        existing.Members = ids.Select(agentId => new AgentTeamMember { TeamId = team.Id, AgentId = agentId }).ToList();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var team = await _dbContext.AgentTeams.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (team is null) return;

        _dbContext.AgentTeams.Remove(team);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureAgentsExistAsync(string[] ids, CancellationToken cancellationToken)
    {
        var found = await _dbContext.AiAgents.CountAsync(agent => ids.Contains(agent.Id), cancellationToken);
        if (found != ids.Length)
        {
            throw new InvalidOperationException("Uno o más agentes seleccionados ya no existen.");
        }
    }
}