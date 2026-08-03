
using FenixEngine.Src.Pages;
using FenixEngine.Src.ViewModels;
using FenixEngine.AppCore;
using FenixEngine.Services;

using Microsoft.Extensions.Logging;


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
			
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        // ViewModel y Vista
        builder.Services.AddTransient<InitialViewModel>();
        builder.Services.AddTransient<InitialPage>();


	#if DEBUG
		builder.Logging.AddDebug();
	#endif
		var app = builder.Build();
		app.InitializeEngineDatabase();


		return app;

	}

}
