namespace FenixEngine.AppCore.Src.Contracts;

public interface IHandler
{
    string Name { get; }
    HandlerState State { get; }
    Task HandleAsync(ServiceContext context);
}
