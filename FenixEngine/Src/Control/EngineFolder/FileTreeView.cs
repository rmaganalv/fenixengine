
using System.Collections.ObjectModel;

namespace FenixEngine.Src.Control;
// Control visual
public class FileTreeView : ContentView
{
    private readonly ObservableCollection<FileNode> _flatList = new();
    private readonly CollectionView _treeView;

    public FileTreeView()
    {
        _treeView = new CollectionView
        {
            ItemsSource = _flatList,
ItemTemplate = new DataTemplate(() =>
{
    var grid = new Grid { Padding = 4 };
    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 30 });
    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

    var icon = new Image { WidthRequest = 20, HeightRequest = 20 };
    icon.SetBinding(Image.SourceProperty, "Icon");

    var label = new Label { VerticalOptions = LayoutOptions.Center };
    label.SetBinding(Label.TextProperty, "Name");
    label.SetBinding(Label.MarginProperty,
        new Binding("Level", converter: new LevelToMarginConverter()));

    grid.Add(icon);
    grid.Add(label, 1, 0);

    // Tap para expandir
    var tap = new TapGestureRecognizer();
    tap.Tapped += async (s, e) =>
    {
        if (grid.BindingContext is FileNode node && node.IsFolder)
        {
            if (!node.IsExpanded)
            {
                node.IsExpanded = true;
                await Task.Run(() => FileTreeLoader.LoadChildren(node));
                MainThread.BeginInvokeOnMainThread(() => RefreshTree());
            }
            else
            {
                node.IsExpanded = false;
                RefreshTree();
            }
        }
    };
    grid.GestureRecognizers.Add(tap);

    return grid;
})

        };

        Content = _treeView;
    }

    public void LoadTree(string path)
    {
        var root = new FileNode
        {
            Name = Path.GetFileName(path),
            Path = path,
            IsFolder = true,
            Level = 0
        };
        _flatList.Clear();
        _flatList.Add(root);
    }

private void RefreshTree()
{
    var newList = new List<FileNode>();
    Flatten(_flatList[0], newList);

    _flatList.Clear();
    foreach (var node in newList)
        _flatList.Add(node);
}


    private void Flatten(FileNode node, List<FileNode> list)
    {
        list.Add(node);
        if (node.IsExpanded)
            foreach (var child in node.Children)
                Flatten(child, list);
    }

public static string GetIconForFile(string filePath)
{
    var ext = Path.GetExtension(filePath).ToLower();
    return ext switch
    {
        ".png" or ".jpg" or ".jpeg" => "image.png",
        ".mp3" or ".wav" => "audio.png",
        ".txt" => "text.png",
        _ => "file.png"
    };
}


}