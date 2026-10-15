using Microsoft.Extensions.DependencyInjection;
using FenixEngine.UI.ModelView;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        var services = Application.Current?.Handler?.MauiContext?.Services ?? throw new InvalidOperationException("Maui services are not available.");
        var viewModel = services.GetRequiredService<SettingsPageViewModel>();
        viewModel.NavigateAsync = async pageType =>
        {
            var page = services.GetRequiredService(pageType) as Page
                ?? throw new InvalidOperationException($"No hay una página registrada para {pageType.Name}.");
            await Navigation.PushAsync(page);
        };
        BindingContext = viewModel;
    }
}
