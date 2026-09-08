

using FenixEngine.Shared.Src.Services;

namespace FenixEngine.AppCore.Src.Contracts;

public class FindFolderHandler : IHandler
{
    private readonly IFolderService _folderService = new ServiceFolder();
    public string Name => "FindFolder";
    public HandlerState State { get; private set; } = HandlerState.Idle;

    public async Task HandleAsync(ServiceContext context)
    {
        try
        {
            State = HandlerState.Running;
            var path = context.GetParameter<string>("path");
            var result = _folderService.FindFolder(path);
            context.SetResult("folderPath", result);
            State = HandlerState.Completed;
            await Task.CompletedTask;
        }
        catch
        {
            State = HandlerState.Failed;
            throw;
        }
    }
}
