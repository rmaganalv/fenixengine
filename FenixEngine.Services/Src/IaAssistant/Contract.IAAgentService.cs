using FenixEngine.Services.Src.Models;

namespace FenixEngine.Services.IaAssistant;

public interface IAIAgentService
{
    Task<string> SendQueryAsync(
        AgentOptions options, 
        string systemInstruction, 
        string userQuery, 
        IEnumerable<string> filePaths);

    Task<List<string>> FetchAvailableModelsAsync(AgentOptions options);
}