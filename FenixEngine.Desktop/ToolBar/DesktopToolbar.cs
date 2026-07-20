namespace FenixEngine.Desktop.ToolBar;

public abstract class DesktopToolbar
{
    protected readonly List<ToolbarItem> Items = new();

    public IReadOnlyCollection<ToolbarItem> ToolbarItems => Items;

    public virtual DesktopToolbar Add(ToolbarItem item)
    {
        Items.Add(item);

        OnItemAdded(item);

        return this;
    }

    protected abstract void OnItemAdded(ToolbarItem item);

    public abstract void Refresh();

    public abstract void Clear();
}