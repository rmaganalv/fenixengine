namespace FenixEngine.Agents.Services.Ollama;

public class ServiceGenerateOllama
{
    private readonly IOllamaClient _client;

    public ServiceGenerateOllama(IOllamaClient client)
    {
        _client = client;
    }

    public async Task<string> GenerateAsync(string prompt, string model)
    {
        var payload = new { model, prompt };
        return await _client.PostJsonAsync("generate", payload);
    }
}
