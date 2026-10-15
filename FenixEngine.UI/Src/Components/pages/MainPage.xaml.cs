using Microsoft.Extensions.DependencyInjection;
using FenixEngine.UI.ModelView;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        var services = Application.Current?.Handler?.MauiContext?.Services ?? throw new InvalidOperationException("Maui services are not available.");
        BindingContext = services.GetRequiredService<MainPageViewModel>();
    }

    private async void OnNewProjectClicked(object sender, EventArgs e)
    {
        var services = Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Maui services are not available.");
        await DevelopmentProjectNavigation.NavigateAsync(services, Navigation, typeof(ProjectCreatePage));
    }

    private async void OnDatabaseClicked(object sender, EventArgs e)
    {
        var services = Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Maui services are not available.");
        await DevelopmentProjectNavigation.NavigateAsync(services, Navigation, typeof(ProjectsPage));
    }

    private async void OnWorkStationClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WorkStationPage());
    }

    private async void OnTasksClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TaskPage());
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}
