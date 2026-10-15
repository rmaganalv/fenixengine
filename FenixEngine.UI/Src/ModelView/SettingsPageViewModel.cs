using System.Collections.ObjectModel;
using System.Windows.Input;
using FenixEngine.AppCore.Controllers;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI.ModelView;

public sealed class SettingsPageViewModel : ViewModelBase
{
    public ObservableCollection<CloudServiceSetting> CloudServices { get; } = new();
    public ObservableCollection<LocalAgentSetting> LocalAgents { get; } = new();
    public ICommand OpenProvidersCommand { get; }
    public ICommand OpenModelsCommand { get; }
    public ICommand OpenAgentsCommand { get; }
    public ICommand OpenTeamsCommand { get; }
    public Func<Type, Task>? NavigateAsync { get; set; }

    public SettingsPageViewModel(ProjectPortfolioController controller)
    {
        OpenProvidersCommand = new Command(async () => await OpenAsync(typeof(ProvidersPage)));
        OpenModelsCommand = new Command(async () => await OpenAsync(typeof(ModelsPage)));
        OpenAgentsCommand = new Command(async () => await OpenAsync(typeof(AgentsPage)));
        OpenTeamsCommand = new Command(async () => await OpenAsync(typeof(TeamsPage)));

        foreach (var service in controller.GetCloudServices())
        {
            CloudServices.Add(service);
        }

        foreach (var agent in controller.GetLocalAgents())
        {
            LocalAgents.Add(agent);
        }
    }

    private Task OpenAsync(Type pageType) => NavigateAsync?.Invoke(pageType) ?? Task.CompletedTask;
}
