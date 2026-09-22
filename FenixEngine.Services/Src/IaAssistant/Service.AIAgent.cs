using System.Text.Json;
using FenixEngine.Service.Src.Prompts;
using FenixEngine.Services.IaAssistant;
using FenixEngine.Services.Src.Models;
using FenixEngine.Services.Src.ApiClient;

namespace FenixEngine.Services.Src.IaAssistant;

public class AIAgentService : IAIAgentService
{
    private readonly IPromptBuilderService _promptBuilder;
    private readonly IHttpClientService _httpClientService;

    public AIAgentService(IPromptBuilderService promptBuilder, IHttpClientService httpClientService)
    {
        _promptBuilder = promptBuilder;
        _httpClientService = httpClientService;
    }

    public async Task<string> SendQueryAsync(
        AgentOptions options, 
        string systemInstruction, 
        string userQuery, 
        IEnumerable<string> filePaths)
    {
        // Carga rápida del contenido de los archivos Markdown
        var mdContents = new List<string>();
        if (filePaths != null)
        {
            foreach (var path in filePaths)
            {
                if (File.Exists(path))
                {
                    var content = await File.ReadAllTextAsync(path);
                    mdContents.Add($"--- ARCHIVO: {Path.GetFileName(path)} ---\n{content}");
                }
            }
        }

        // Construcción del payload
        var payload = _promptBuilder.BuildChatPayload(systemInstruction, userQuery, mdContents, options);
        var endpoint = options.BaseUrl.TrimEnd('/') + "/chat/completions";

        // Petición HTTP
        var jsonResponse = await _httpClientService.PostJsonAsync(endpoint, payload, options.ApiKey);

        // Parsing básico del Response estándar v1
        using var doc = JsonDocument.Parse(jsonResponse);
        var root = doc.RootElement;

        if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
        {
            var firstChoice = choices[0];
            if (firstChoice.TryGetProperty("message", out var message) && message.TryGetProperty("content", out var content))
            {
                return content.GetString() ?? string.Empty;
            }
        }

        return jsonResponse; // Retorna la respuesta completa si difiere la estructura
    }

    public async Task<List<string>> FetchAvailableModelsAsync(AgentOptions options)
    {
        try
        {
            var endpoint = options.BaseUrl.TrimEnd('/') + "/models";
            using var doc = await _httpClientService.GetJsonAsync<JsonDocument>(endpoint, options.ApiKey);
            
            var modelsList = new List<string>();
            if (doc != null && doc.RootElement.TryGetProperty("data", out var dataArray))
            {
                foreach (var modelItem in dataArray.EnumerateArray())
                {
                    if (modelItem.TryGetProperty("id", out var idElement))
                    {
                        modelsList.Add(idElement.GetString()!);
                    }
                }
            }
            return modelsList.Count > 0 ? modelsList : new List<string> { options.SelectedModel };
        }
        catch
        {
            // Fallback en caso de que el endpoint /models de la API no esté expuesto
            return new List<string> { options.SelectedModel, "gpt-4o", "claude-3-5-sonnet", "llama3" };
        }
    }
}
