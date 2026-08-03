namespace FenixEngine.Desktop.Window;

public abstract class DesktopWindow
{
    public abstract string Title { get; set; }

    public abstract void Center();

    public abstract void Maximize();

    public abstract void Minimize();

    public abstract void ToggleFullScreen();
}