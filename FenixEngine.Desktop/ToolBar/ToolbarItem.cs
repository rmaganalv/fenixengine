namespace FenixEngine.Desktop.ToolBar;

public abstract class ToolbarItem
{
    public string Identifier { get; }

    protected ToolbarItem(string identifier)
    {
        Identifier = identifier;
    }
}