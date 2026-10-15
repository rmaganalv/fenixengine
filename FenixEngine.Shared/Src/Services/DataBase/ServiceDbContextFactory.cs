using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Shared.Src.Services.DataBase;

public sealed class ServiceDbContextFactory : IDesignTimeDbContextFactory<ServiceDbContext>
{
    public ServiceDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ServiceDbContext>()
            .UseSqlite($"Data Source={Path.Combine(Path.GetTempPath(), "fenixengine-design-time.db3")}")
            .Options;

        return new ServiceDbContext(options);
    }
}