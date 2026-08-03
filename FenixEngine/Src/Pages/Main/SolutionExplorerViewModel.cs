using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

using FenixEngine.Src.Control;

namespace FenixEngine.Src.Pages;
public class SolutionExplorerViewModel : INotifyPropertyChanged
{
    public ObservableCollection<SolutionItem> Items { get; set; } = new();

    private ObservableCollection<SolutionItem> _flatItems = new();
    public ObservableCollection<SolutionItem> FlatItems
    {
        get => _flatItems;
        set { _flatItems = value; OnPropertyChanged(nameof(FlatItems)); }
    }

public void LoadSolution(string rootPath)
{
    Items.Clear();
    var root = BuildTree(rootPath);
    root.IsExpanded = true; // fuerza expansión inicial
    Items.Add(root);
    RefreshExplorer();
}

private SolutionItem BuildTree(string rootPath)
{
    var root = new SolutionItem
    {
        Name = Path.GetFileName(rootPath),
        Path = rootPath,
        IsFolder = true,
        Icon = "folder.png"
    };

    // Agregar subcarpetas
    foreach (var dir in Directory.GetDirectories(rootPath))
        root.Children.Add(BuildTree(dir));

    // Agregar archivos
    foreach (var file in Directory.GetFiles(rootPath))
    {
        root.Children.Add(new SolutionItem
        {
            Name = Path.GetFileName(file),
            Path = file,
            IsFolder = false,
            Icon = GetIconForFile(file)
        });
    }

    return root;
}

    private string GetIconForFile(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLower();
        return ext switch
        {
            ".cs"   => "csharp.png",
            ".xaml" => "xaml.png",
            //".json" => "json.png",
            //".xml"  => "xml.png",
            ".csproj" => "docfolder.png",
            _       => "file.png"
        };
    }

    public void OnItemTapped(SolutionItem item)
    {
        if (item.IsFolder)
        {
            item.IsExpanded = !item.IsExpanded;
            RefreshExplorer();
        }
    }

private void RefreshExplorer()
{
    var flatList = new ObservableCollection<SolutionItem>();
    Flatten(Items, flatList);
    Debug.WriteLine($"Se encontraron {flatList.Count} elementos");
    FlatItems = flatList;
}


private void Flatten(IEnumerable<SolutionItem> source, ObservableCollection<SolutionItem> target)
{
    foreach (var item in source)
    {
        target.Add(item);
        if (item.IsFolder && item.IsExpanded)
            Flatten(item.Children, target);
    }
}


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
