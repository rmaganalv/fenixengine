
using FenixEngine.Src.Pages;
using FenixEngine.Src.ViewModels;
using FenixEngine.AppCore;
using FenixEngine.Services;
using FenixEngine.Shared;
using FenixEngine.DataBase;

using Microsoft.Extensions.Logging;
using FenixEngine.Src.Pages.Chat;


namespace FenixEngine;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseEngineAppCore()
			.UseEngineServices()///TODO: Eliminate this Service
			.UseDataBaseServices()
			
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			
			.UsePages();

	#if DEBUG
		builder.Logging.AddDebug();
	#endif
		var app = builder.Build();
		app.InitializeEngineDatabase();
		return app;

	}

	private static MauiAppBuilder UsePages(this MauiAppBuilder builder)
	{
		// ViewModel y Vista
		builder.Services.AddSingleton<ChatViewModel>();
        builder.Services.AddSingleton<ChatPage>();
        builder.Services.AddTransient<InitialViewModel>();
        builder.Services.AddTransient<InitialPage>();
		return builder;
	}

}
