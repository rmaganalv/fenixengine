using FenixEngine.UI.ModelView;
using Microsoft.Extensions.DependencyInjection;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI;

public partial class ProjectCreatePage : ContentPage, IDevelopmentProjectRoutePage
{
    private readonly ProjectCreatePageViewModel _viewModel;

    public ProjectCreatePage()
    {
        InitializeComponent();
        var services = Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Maui services are not available.");
        _viewModel = services.GetRequiredService<ProjectCreatePageViewModel>();
        _viewModel.NavigateAsync = (pageType, routeId) => DevelopmentProjectNavigation.NavigateAsync(services, Navigation, pageType, routeId);
        BindingContext = _viewModel;
    }

    public Task LoadRouteAsync(string? routeId) => _viewModel.LoadAsync(routeId);
}