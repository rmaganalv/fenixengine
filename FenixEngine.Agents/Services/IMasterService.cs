
namespace FenixEngine.Agents.Services;
public interface IMasterService
{
    Task<IEnumerable<string>> ListModelsAsync();
    Task<string> GenerateAsync(string prompt, string model);
    Task<string> MixModelsAsync(IEnumerable<string> models);
    Task<string> CreateEmbeddingsAsync(string text, string model);
}
