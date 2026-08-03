

namespace FenixEngine.Agents.Services.Ollama;

public class ServiceMixOllama
{
    private readonly IOllamaClient _client;

    public ServiceMixOllama(IOllamaClient client)
    {
        _client = client;
    }

    public async Task<string> MixModelsAsync(IEnumerable<string> models)
    {
        var payload = new { models };
        return await _client.PostJsonAsync("mix", payload);
    }
}
