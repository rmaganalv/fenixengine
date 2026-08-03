
namespace FenixEngine.Shared.Services
{
    public interface IFileService
    {
        Task CreateFile(string? pathFile);
        Task DeleteFile(string? pathFile);
        Task FindFile(string? pathFile);
        Task EditFile(string? pathFile, string? newContent);
    }

    public interface IFolderService
    {
        Task CreateFolder(string? pathFolder);
        Task DeleteFolder(string? pathFolder);
        Task FindFolder(string? pathFolder);
        Task EditFolder(string? pathFolder, string? newPathFolder);

    } 

    public interface ITerminalService
    {
        Task<string> ExecuteCommandAsync(string? command);
    }
}
