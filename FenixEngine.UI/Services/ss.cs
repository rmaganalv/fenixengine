using FenixEngine.Desktop.ToolBar;
using FenixEngine.Desktop.Window;

namespace FenixEngine.Desktop.Services;
public class DesktopService : IDesktopService
{
    public DesktopService(
        DesktopWindow window,
        DesktopToolbar toolbar)
    {
        Window = window;
        Toolbar = toolbar;
    }

    public DesktopWindow Window { get; }

    public DesktopToolbar Toolbar { get; }
}