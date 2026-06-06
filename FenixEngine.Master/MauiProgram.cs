using FenixEngine.ShellService;
using FenixEngine.ShellCore.Harness;
using FenixEngine.ShellCore.Mcp;
using FenixEngine.ShellApp;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace FenixEngine.Master;
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
			.UseShellMcp()
			.UseShellHarness()
			.UseShellApp()
			.UseShellServices()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}
