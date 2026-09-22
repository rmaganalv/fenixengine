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
using FenixEngine.Shared.Modules;
using FenixEngine.Shared.Src.Repositories;
using FenixEngine.Shared.Src.Services.DataBase;
using FenixEngine.Shared.Src.Services;
using Microsoft.EntityFrameworkCore;

namespace FenixEngine.Shared;

public static class SharedExtension
{
    public static MauiAppBuilder UseSharedServices(this MauiAppBuilder builder)
    {

        builder.Services.AddSingleton<IFileService,ServiceFile>();
        builder.Services.AddSingleton<IFolderService,ServiceFolder>();
        builder.Services.AddSingleton<ITerminalService,ServiceTerminal>();
        builder.Services.AddSingleton<ILoggerService,ServiceFileLogger>();
        builder.Services.AddDbContext<ServiceDbContext>(options =>
        {
            string databasePath = Path.Combine(FileSystem.AppDataDirectory, "fenix-engine.db3");
            options.UseSqlite($"Data Source={databasePath}");
        });
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();


        return builder;
    }

}