namespace FenixEngine.Agents.Services.Gemini;
public class ServiceModelsGemini
{
    private readonly IGeminiClient _client;

    public ServiceModelsGemini(IGeminiClient client)
    {
        _client = client;
    }

    public async Task<string> ListModelsAsync(string apiKey)
    {
        return await _client.GetAsync("models", apiKey);
    }
}

