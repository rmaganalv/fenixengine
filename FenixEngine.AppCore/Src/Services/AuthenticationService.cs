using System.Security.Cryptography;
using System.Text;
using FenixEngine.Shared.Src.Models;
using FenixEngine.Shared.Src.Repositories;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.AppCore.Src.Services;

public sealed class AuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<UserArchitect?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return Task.FromResult<UserArchitect?>(null);
        }

        var passwordHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password))).ToLowerInvariant();
        return _userRepository.AuthenticateAsync(email.Trim().ToLowerInvariant(), passwordHash, cancellationToken);
    }
}