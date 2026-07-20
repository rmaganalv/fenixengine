using System.Collections.ObjectModel;

namespace FenixEngine.Src.Control;
    // Modelo de nodo
    public class FileNode
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
        public bool IsFolder { get; set; }
        public ObservableCollection<FileNode> Children { get; set; } = new();
        public string? Icon { get; set; }
        public bool IsExpanded { get; set; } = false;
    }
