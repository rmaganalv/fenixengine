

namespace FenixEngine.AppCore.Src.Contracts;

public class SaveToDatabaseHandler : IHandler
{
    //private readonly CrudProjectService _crudService = new();
    public string Name => "SaveToDatabase";
    public HandlerState State { get; private set; } = HandlerState.Idle;

    public async Task HandleAsync(ServiceContext context)
    {
        try
        {
            State = HandlerState.Running;
            var code = context.GetParameter<string>("generatedCode");
            //_crudService.SaveProject(code);
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
