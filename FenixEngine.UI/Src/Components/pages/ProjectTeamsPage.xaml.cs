using FenixEngine.UI.ModelView;
using Microsoft.Extensions.DependencyInjection;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI;

public partial class ProjectTeamsPage : ContentPage, IDevelopmentProjectRoutePage
{
    private readonly ProjectTeamsPageViewModel _viewModel;

    public ProjectTeamsPage()
    {
        InitializeComponent();
        var services = Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Maui services are not available.");
        _viewModel = services.GetRequiredService<ProjectTeamsPageViewModel>();
        BindingContext = _viewModel;
    }

    public Task LoadRouteAsync(string? routeId) => _viewModel.LoadAsync(routeId);
}