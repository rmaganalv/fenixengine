using FenixEngine.Desktop.Window;
using UIKit;

namespace FenixEngine.Desktop.Platforms.MacCatalyst;

public sealed class MacDesktopWindow : DesktopWindow
{
    public override string Title
    {
        get
        {
            var scene = UIApplication.SharedApplication
                .ConnectedScenes
                .OfType<UIWindowScene>()
                .FirstOrDefault();

            return scene?.Title ?? string.Empty;
        }
        set
        {
            var scene = UIApplication.SharedApplication
                .ConnectedScenes
                .OfType<UIWindowScene>()
                .FirstOrDefault();

            if (scene != null)
                scene.Title = value;
        }
    }

    public override void Center()
    {
        // Lo implementaremos después usando NSWindow
    }

    public override void Maximize()
    {
        // TODO
    }

    public override void Minimize()
    {
        // TODO
    }

    public override void ToggleFullScreen()
    {
        // TODO
    }
}