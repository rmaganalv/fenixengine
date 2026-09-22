using FenixEngine.AppCore.Controllers;
using FenixEngine.Shared.Src.Repositories;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.AppCore.Src.Services;

public sealed class SettingsDataService
{
    private readonly ISettingsRepository _settingsRepository;

    public SettingsDataService(ISettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    public async Task<List<CloudServiceSetting>> GetCloudServicesAsync(CancellationToken cancellationToken = default)
    {
        var services = await _settingsRepository.GetCloudServicesAsync(cancellationToken);

        return services.Select(service => new CloudServiceSetting
        {
            Name = service.ProjectName ?? service.ProviderName,
            Provider = service.ProviderName,
            Enabled = service.IsActive && !string.IsNullOrWhiteSpace(service.Token)
        }).ToList();
    }

    public async Task<List<LocalAgentSetting>> GetLocalAgentsAsync(CancellationToken cancellationToken = default)
    {
        var agents = await _settingsRepository.GetLocalAgentsAsync(cancellationToken);

        return agents.Select(agent => new LocalAgentSetting
        {
            Name = agent.Name,
            Endpoint = agent.Endpoint,
            Enabled = agent.IsEnabled
        }).ToList();
    }
}