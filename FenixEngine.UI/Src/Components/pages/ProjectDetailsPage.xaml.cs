using FenixEngine.UI.ModelView;
using Microsoft.Extensions.DependencyInjection;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI;

public partial class ProjectDetailsPage : ContentPage, IDevelopmentProjectRoutePage
{
    private readonly ProjectDetailsPageViewModel _viewModel;

    public ProjectDetailsPage()
    {
        InitializeComponent();
        var services = Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Maui services are not available.");
        _viewModel = services.GetRequiredService<ProjectDetailsPageViewModel>();
        _viewModel.NavigateAsync = (pageType, routeId) => DevelopmentProjectNavigation.NavigateAsync(services, Navigation, pageType, routeId);
        _viewModel.GoToProjectsAsync = async () => { await Navigation.PopAsync(); };
        _viewModel.ConfirmDeleteAsync = () => DisplayAlertAsync("Eliminar proyecto", "¿Eliminar este proyecto y sus artefactos?", "Eliminar", "Cancelar");
        BindingContext = _viewModel;
    }

    public Task LoadRouteAsync(string? routeId) => _viewModel.LoadAsync(routeId);

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.RefreshAsync();
    }
}