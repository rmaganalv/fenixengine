// Copyright (C) 2026 Ruben Magaña Alvarado
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
using FenixEngine.DataBase.Src.Services;
using Microsoft.EntityFrameworkCore;

namespace FenixEngine.Shared;

public static class DataBaseExtension
{
    public static MauiAppBuilder UseDataBaseServices(this MauiAppBuilder builder)
    {

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "FenixEngine.db3");
       

        builder.Services.AddDbContext<ServiceDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}")
        );

        return builder;
    }

    // 2. Método de extensión sobre MauiApp para inicializar la DB al arrancar
    public static MauiApp InitializeEngineDatabase(this MauiApp app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
            // 1. Elimina la base de datos física y todas sus tablas si ya existen
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();
        }

        return app;
    }
}