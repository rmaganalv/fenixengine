using FenixEngine.Desktop.ToolBar;
using FenixEngine.Desktop.Window;

namespace FenixEngine.Desktop;

public static class Desktop
{
    public static DesktopWindow Window { get; private set; } = null!;

    public static DesktopToolbar Toolbar { get; private set; } = null!;

    public static void Initialize()
    {
#if MACCATALYST
        Window  = new Platforms.MacCatalyst.MacDesktopWindow();
        Toolbar = new Platforms.MacCatalyst.MacDesktopToolbar();
#elif WINDOWS
        throw new NotImplementedException();
#else
        throw new PlatformNotSupportedException();
#endif
    }
}
