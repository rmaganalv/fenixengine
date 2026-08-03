using System.Text.Json;

namespace FenixEngine.Agents.Services.Ollama;
public class ServiceModelsOllama
{
    private readonly IOllamaClient _client;

    public ServiceModelsOllama(IOllamaClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<string>> ListModelsAsync()
    {
        var response = await _client.GetAsync("tags");
        var models = JsonSerializer.Deserialize<IEnumerable<string>>(response);
        return models ?? Enumerable.Empty<string>();
    }
}
