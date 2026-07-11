// Copyright (C) 2026 Ruben Magaña Alvarado
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
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
