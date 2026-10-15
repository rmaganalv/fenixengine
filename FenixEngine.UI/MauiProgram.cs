// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

using FenixEngine.AppCore;
using FenixEngine.Services;
using FenixEngine.Shared;
using FenixEngine.Shared.Src.Services.DataBase;
using FenixEngine.UI.ModelView;
using Microsoft.Extensions.Logging;
#if MAUI_DEVFLOW
using Microsoft.Maui.DevFlow.Agent;
#endif

namespace FenixEngine.UI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.UseSharedServices();
		builder.UseEngineServices();
		builder.UseEngineAppCore();
#if MAUI_DEVFLOW
		builder.AddMauiDevFlowAgent();
#endif
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<WorkStationPageViewModel>();
		builder.Services.AddTransient<TaskPageViewModel>();
		builder.Services.AddTransient<SettingsPageViewModel>();
		builder.Services.AddTransient<LoginPageViewModel>();
		builder.Services.AddTransient<ProvidersPageViewModel>();
		builder.Services.AddTransient<ModelsPageViewModel>();
		builder.Services.AddTransient<AgentsPageViewModel>();
		builder.Services.AddTransient<TeamsPageViewModel>();
		builder.Services.AddTransient<ProvidersPage>();
		builder.Services.AddTransient<ModelsPage>();
		builder.Services.AddTransient<AgentsPage>();
		builder.Services.AddTransient<TeamsPage>();
#if DEBUG
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();
		using (var scope = app.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
			DatabaseInitializer.Initialize(dbContext);
		}

		return app;
	}
}
