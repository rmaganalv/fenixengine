using FenixEngine.Services.Src.Models;

namespace FenixEngine.Service.Src.Prompts;

public class PromptBuilderService : IPromptBuilderService
{
    public object BuildChatPayload(
        string systemInstruction, 
        string userQuery, 
        IEnumerable<string> markdownFilesContent, 
        AgentOptions options)
    {
        var messages = new List<object>();

        // 1. Mensaje de Sistema
        if (!string.IsNullOrWhiteSpace(systemInstruction))
        {
            messages.Add(new { role = "system", content = systemInstruction });
        }

        // 2. Adjuntar contexto de archivos MD
        var contextText = string.Empty;
        if (markdownFilesContent != null && markdownFilesContent.Any())
        {
            contextText = "### CONTEXTO ADICIONAL (.md):\n" + 
                string.Join("\n\n---\n\n", markdownFilesContent) + "\n\n";
        }

        // 3. Consulta del Usuario combinada con contexto
        var fullUserContent = $"{contextText}### INSTRUCCIÓN DEL USUARIO:\n{userQuery}";
        messages.Add(new { role = "user", content = fullUserContent });

        // Retorna payload compatible con OpenAI Chat Completions API v1
        return new
        {
            model = options.SelectedModel,
            messages = messages,
            temperature = options.Temperature,
            max_tokens = options.MaxTokens,
            stream = false
        };
    }
}
