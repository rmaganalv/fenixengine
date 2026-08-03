
namespace FenixEngine.Agents.Services;

public interface IOllamaClient
{
    Task<string> PostJsonAsync(string endpoint, object payload, string? apiKey = null);
    Task<string> GetAsync(string endpoint, string? apiKey = null);
}

public interface IGeminiClient
{
    Task<string> PostJsonAsync(string endpoint, object payload, string? apiKey = null);
    Task<string> GetAsync(string endpoint, string? apiKey = null);
}

public interface IAnthropicClient
{
    Task<string> PostJsonAsync(string endpoint, object payload, string apiKey);
    Task<string> GetAsync(string endpoint, string apiKey);
}

public interface IOpenAIClient
{
    Task<string> PostJsonAsync(string endpoint, object payload, string apiKey);
    Task<string> GetAsync(string endpoint, string apiKey);
}

