using System.Collections.ObjectModel;
using FenixEngine.AppCore.Controllers;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.ModelView;

public sealed class SettingsPageViewModel : ViewModelBase
{
    public ObservableCollection<CloudServiceSetting> CloudServices { get; } = new();
    public ObservableCollection<LocalAgentSetting> LocalAgents { get; } = new();

    public SettingsPageViewModel(ProjectPortfolioController controller)
    {
        foreach (var service in controller.GetCloudServices())
        {
            CloudServices.Add(service);
        }

        foreach (var agent in controller.GetLocalAgents())
        {
            LocalAgents.Add(agent);
        }
    }
}
