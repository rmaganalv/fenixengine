namespace FenixEngine.AppCore.Src.Contracts;

public class GenerateCodeHandler : IHandler
{
    public string Name => "GenerateCode";
    public HandlerState State { get; private set; } = HandlerState.Idle;

    public async Task HandleAsync(ServiceContext context)
    {
        try
        {
            State = HandlerState.Running;
            var prompt = context.GetParameter<string>("prompt");
            var code = $"// Código generado para: {prompt}";
            context.SetResult("generatedCode", code);
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
