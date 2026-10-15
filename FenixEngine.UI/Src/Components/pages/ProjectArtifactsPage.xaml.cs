using FenixEngine.UI.ModelView;
using Microsoft.Extensions.DependencyInjection;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI;

public partial class ProjectArtifactsPage : ContentPage, IDevelopmentProjectRoutePage
{
    private readonly ProjectArtifactsPageViewModel _viewModel;

    public ProjectArtifactsPage()
    {
        InitializeComponent();
        var services = Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Maui services are not available.");
        _viewModel = services.GetRequiredService<ProjectArtifactsPageViewModel>();
        BindingContext = _viewModel;
    }

    public Task LoadRouteAsync(string? routeId) => _viewModel.LoadAsync(routeId);
}