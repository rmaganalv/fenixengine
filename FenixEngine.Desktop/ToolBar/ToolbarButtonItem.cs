namespace FenixEngine.Desktop.ToolBar;

public class ToolbarButtonItem : ToolbarItem
{
    public string? Text { get; }

    public string? Image { get; }

    public Action? Clicked { get; }

    public ToolbarButtonItem(
        string id,
        string text,
        Action? clicked = null,
        string? image = null)
        : base(id)
    {
        Text = text;
        Clicked = clicked;
        Image = image;
    }
}