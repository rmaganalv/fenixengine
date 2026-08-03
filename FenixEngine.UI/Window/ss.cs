
namespace FenixEngine.Desktop;
public interface IDesktopWindow
{
    string Title { get; set; }

    void Center();

    void Maximize();

    void Minimize();

    void ToggleFullScreen();
}