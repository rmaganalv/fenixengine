
namespace AppCore.Contracts;

public interface IAppService
{
    string Name { get; }
    string Category { get; } // Ej: "IA", "Database", "Shared"
    
    Task ExecuteAsync(ServiceContext context);
}