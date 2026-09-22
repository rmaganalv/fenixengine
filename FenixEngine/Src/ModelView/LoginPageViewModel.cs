using System.Windows.Input;
using FenixEngine.AppCore.Src.Services;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.ModelView;

public sealed class LoginPageViewModel : ViewModelBase
{
    private readonly AuthenticationService _authenticationService;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }

    public ICommand LoginCommand { get; }
    public Func<Task>? OnAuthenticated { get; set; }

    public LoginPageViewModel(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        LoginCommand = new Command(async () => await LoginAsync());
    }

    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Ingresa correo y contraseña para continuar.";
            return;
        }

        var user = await _authenticationService.AuthenticateAsync(Email, Password);
        if (user is null)
        {
            ErrorMessage = "El correo o la contraseña no son válidos.";
            return;
        }

        if (OnAuthenticated is not null)
        {
            await OnAuthenticated();
        }
    }
}
