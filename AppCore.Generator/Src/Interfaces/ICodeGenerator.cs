namespace AppCore.Generator;

public interface ICodeGenerator
{
    Task<string> GenerateCodeAsync(string prompt);
}