using FenixEngine.Desktop.ToolBar;
using UIKit;

namespace FenixEngine.Desktop.Platforms.MacCatalyst;

public sealed class MacDesktopToolbar : DesktopToolbar
{
    protected override void OnItemAdded(FenixEngine.Desktop.ToolBar.ToolbarItem item)
    {
        // Aquí construiremos el NSToolbarItem correspondiente
    }

    public override void Refresh()
    {
    }

    public override void Clear()
    {
        Items.Clear();
    }

    public void ShowTestToolbar()
    {
        // Crear NSToolbar

        // Agregar un botón

        // Asignarla al NSWindow

        var uiWindow = UIApplication.SharedApplication
            .ConnectedScenes
            .OfType<UIWindowScene>()
            .FirstOrDefault()?
            .Windows
            .FirstOrDefault();


    }


}
