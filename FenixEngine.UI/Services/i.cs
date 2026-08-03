using FenixEngine.Desktop.ToolBar;
using FenixEngine.Desktop.Window;

namespace FenixEngine.Desktop.Services;

public interface IDesktopService
{
    DesktopWindow Window { get; }

    DesktopToolbar Toolbar { get; }
}