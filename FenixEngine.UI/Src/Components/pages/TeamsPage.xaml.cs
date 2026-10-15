using FenixEngine.UI.ModelView;
using Microsoft.Extensions.DependencyInjection;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI;

public partial class TeamsPage : ContentPage
{
    public TeamsPage()
    {
        InitializeComponent();
        var services = Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Maui services are not available.");
        BindingContext = services.GetRequiredService<TeamsPageViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((TeamsPageViewModel)BindingContext).LoadAsync();
    }
}