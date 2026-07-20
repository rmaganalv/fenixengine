using FenixEngine.Desktop.Platforms.MacCatalyst;
using FenixEngine.Desktop.Services;
using FenixEngine.Desktop.ToolBar;
using FenixEngine.Desktop.Window;

namespace FenixEngine.Desktop;

public static class DesktopExtensions
{
    public static MauiAppBuilder AddDesktop(this MauiAppBuilder builder)
    {
#if MACCATALYST
        builder.Services.AddSingleton<DesktopWindow, FenixEngine.Desktop.Platforms.MacCatalyst.MacDesktopWindow>();
        builder.Services.AddSingleton<DesktopToolbar, FenixEngine.Desktop.Platforms.MacCatalyst.MacDesktopToolbar>();
#elif WINDOWS
        // builder.Services.AddSingleton<DesktopWindow, WindowsDesktopWindow>();
        // builder.Services.AddSingleton<DesktopToolbar, WindowsDesktopToolbar>();
#endif

        builder.Services.AddSingleton<IDesktopService, DesktopService>();

        return builder;
    }
}