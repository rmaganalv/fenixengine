
using FenixEngine.Desktop;
using Microsoft.Extensions.Logging;

namespace FenixEngine;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			//.AddDesktop()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});



#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

}
