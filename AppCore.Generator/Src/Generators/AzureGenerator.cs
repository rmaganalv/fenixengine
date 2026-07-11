
using AppCore.Generator.Generators.Interfaces;
using Microsoft.Extensions.AI;
using OpenAI.Chat;


namespace AppCore.Generator.Generators;

public class AzureGenerator: IGenerator
{
    private readonly IChatClient? _chatClient;

    public AzureGenerator(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }


    public async Task<string> ProcessTaskAsync(string systemPrompt, string userPrompt)
    {
        var chatMessages = new List<Microsoft.Extensions.AI.ChatMessage>
        {
            new(ChatRole.System, systemPrompt),
            new(ChatRole.User, userPrompt)
        };

        // Ejecución estándar unificada
        // Firma simplificada usando el método de extensión para strings directos:
        ChatResponse response = await _chatClient.GetResponseAsync(chatMessages, new ChatOptions());
        return response.Messages.ToString() ?? "No se generó respuesta.";
    }
}
