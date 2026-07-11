
namespace AppCore.Generator.Generators.Interfaces;

public interface IGenerator
{
    Task<string> ProcessTaskAsync(string systemPrompt, string userPrompt);
}