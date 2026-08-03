using System.Collections.ObjectModel;

namespace FenixEngine.Src.Control;
public class SolutionItem
{
    public string? Name { get; set; }
    public string? Path { get; set; }   // Ruta completa en disco
    public bool IsFolder { get; set; }
    public bool IsExpanded { get; set; }
    public string? Icon { get; set; }
    public ObservableCollection<SolutionItem> Children { get; set; } = new();
}
