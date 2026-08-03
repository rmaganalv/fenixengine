namespace FenixEngine.Desktop.ToolBar;

public class ToolbarSearchItem : ToolbarItem
{
    public string Placeholder { get; }

    public Action<string>? SearchChanged { get; }

    public ToolbarSearchItem(
        string id,
        string placeholder,
        Action<string>? changed = null)
        : base(id)
    {
        Placeholder = placeholder;
        SearchChanged = changed;
    }
}