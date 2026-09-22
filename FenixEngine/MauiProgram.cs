// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

using FenixEngine.AppCore;
using FenixEngine.Services;
using FenixEngine.Shared;
using FenixEngine.Shared.Src.Services.DataBase;
using FenixEngine.ModelView;
using Microsoft.Extensions.Logging;

namespace FenixEngine;

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
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<WorkStationPageViewModel>();
		builder.Services.AddTransient<TaskPageViewModel>();
		builder.Services.AddTransient<SettingsPageViewModel>();
		builder.Services.AddTransient<LoginPageViewModel>();
		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();
		using (var scope = app.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
			#if DEBUG
			DatabaseInitializer.Recreate(dbContext);
			#else
			DatabaseInitializer.Initialize(dbContext);
			#endif
		}

		return app;
	}
}
