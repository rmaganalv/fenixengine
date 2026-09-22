using Microsoft.Extensions.DependencyInjection;
using FenixEngine.ModelView;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine;

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
        await DisplayAlertAsync("Proyecto", "Se abrirá la creación de un nuevo proyecto.", "Aceptar");
    }

    private async void OnDatabaseClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Base de datos", "Se gestionará la persistencia de proyectos y actividades.", "Aceptar");
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
