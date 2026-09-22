using Microsoft.Extensions.DependencyInjection;
using FenixEngine.ModelView;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        var services = Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Maui services are not available.");
        var viewModel = services.GetRequiredService<LoginPageViewModel>();
        viewModel.OnAuthenticated = () => Navigation.PushAsync(new MainPage());
        BindingContext = viewModel;
    }
}
