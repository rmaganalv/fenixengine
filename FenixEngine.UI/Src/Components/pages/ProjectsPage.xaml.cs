using FenixEngine.UI.ModelView;
using Microsoft.Extensions.DependencyInjection;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI;

public partial class ProjectsPage : ContentPage
{
    public ProjectsPage()
    {
        InitializeComponent();
        var services = Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Maui services are not available.");
        var viewModel = services.GetRequiredService<ProjectsPageViewModel>();
        viewModel.NavigateAsync = (pageType, routeId) => DevelopmentProjectNavigation.NavigateAsync(services, Navigation, pageType, routeId);
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ProjectsPageViewModel)BindingContext).LoadAsync();
    }
}