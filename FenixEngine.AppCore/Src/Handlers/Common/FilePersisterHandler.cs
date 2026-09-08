
using FenixEngine.AppCore.Utils.Generators;

namespace FenixEngine.AppCore.Nodes.Common;

public class FilePersisterHandler
{
    public async Task PersistFragmentsAsync(string fullCode, string targetDirectory, Action<string> onFileGenerated)
    {
        Directory.CreateDirectory(targetDirectory);

        var fragments = Sanitizant.SplitByTypes(fullCode);
        int counter = 1;

        foreach (var fragment in fragments)
        {
            string fileName = ResolveFileName(fragment, counter);
            string filePath = Path.Combine(targetDirectory, fileName);

            await File.WriteAllTextAsync(filePath, fragment.Trim());
            onFileGenerated?.Invoke(filePath);
            counter++;
        }
    }

    private static string ResolveFileName(string fragment, int defaultIndex)
    {
        string fileName = $"Generated{defaultIndex}.cs";

        if (fragment.Contains("class"))
        {
            var name = Sanitizant.ExtractName(fragment, "class");
            if (!string.IsNullOrEmpty(name)) fileName = $"{name}.cs";
        }
        else if (fragment.Contains("enum"))
        {
            var name = Sanitizant.ExtractName(fragment, "enum");
            if (!string.IsNullOrEmpty(name)) fileName = $"{name}.cs";
        }
        else if (fragment.Contains("interface"))
        {
            var name = Sanitizant.ExtractName(fragment, "interface");
            if (!string.IsNullOrEmpty(name)) fileName = $"{name}.cs";
        }

        return fileName;
    }
}