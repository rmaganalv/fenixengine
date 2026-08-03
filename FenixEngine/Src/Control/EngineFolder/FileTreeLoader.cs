
namespace FenixEngine.Src.Control;
// Servicio de carga de árbol
public static class FileTreeLoader
{

    
    public static FileNode Load(string rootPath)
    {
        var root = new FileNode
        {
            Name = Path.GetFileName(rootPath),
            Path = rootPath,
            IsFolder = true,
            Icon = "folder.png"
        };

        LoadChildren(root);
        return root;
    }

/*
    public static void LoadChildren(FileNode node)
        {
            if (!node.IsFolder) return;

            foreach (var dir in Directory.GetDirectories(node.Path ?? string.Empty))
            {
                var child = new FileNode
                {
                    Name = Path.GetFileName(dir),
                    Path = dir,
                    IsFolder = true,
                    Icon = "folder.png"
                };
                node.Children.Add(child);
            }

            foreach (var file in Directory.GetFiles(node.Path ?? string.Empty))
            {
                node.Children.Add(new FileNode
                {
                    Name = Path.GetFileName(file),
                    Path = file,
                    IsFolder = false,
                    Icon = GetIconForFile(file)
                });
            }
        }
*/
    private static string GetIconForFile(string file)
    {
        string ext = Path.GetExtension(file).ToLower();
        return ext switch
        {
            ".cs" => "csharp.png",
            ".doc" => "docfolder.png",
            ".png" => "image.png",
            _ => "file.png"
        };
    }

    public static void LoadChildren(FileNode node)
    {
        if (!node.IsFolder) return;

        foreach (var dir in Directory.GetDirectories(node.Path ?? string.Empty))
        {
            var child = new FileNode
            {
                Name = Path.GetFileName(dir),
                Path = dir,
                IsFolder = true,
                Icon = "folder.png",
                Level = node.Level + 1
            };
            node.Children.Add(child);
        }

        foreach (var file in Directory.GetFiles(node.Path ?? string.Empty))
        {
            node.Children.Add(new FileNode
            {
                Name = Path.GetFileName(file),
                Path = file,
                IsFolder = false,
                Icon = GetIconForFile(file),
                Level = node.Level + 1
            });
        }
    }


}