using Service.Resources.Contracts;
using Service.Resources.SystemFiles;

namespace FenixEngine.AppCore.Nodes.Common;

public class FilePickerHandler
{
    private readonly ISystemFiles _systemFiles;

    public FilePickerHandler(ISystemFiles? systemFiles = null)
    {
        _systemFiles = systemFiles ?? new SystemFiles();
    }

    public async Task<string?> PickFileAsync()
    {
        string selectedFile = await _systemFiles.PickFile(Environment.CurrentDirectory);
        return string.IsNullOrEmpty(selectedFile) ? null : selectedFile;
    }
}

