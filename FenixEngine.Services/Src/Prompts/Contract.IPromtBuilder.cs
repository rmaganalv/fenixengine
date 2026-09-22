using FenixEngine.Services.Src.Models;


namespace FenixEngine.Service.Src.Prompts;
public interface IPromptBuilderService
{
    object BuildChatPayload(
        string systemInstruction, 
        string userQuery, 
        IEnumerable<string> markdownFilesContent, 
        AgentOptions options);
}