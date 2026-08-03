namespace FenixEngine.Agents.Services.Gemini;
public class ServiceTextGemini
{
    private readonly IGeminiClient _client;
    private readonly string _model;

    public ServiceTextGemini(IGeminiClient client, string model = "gemini-pro")
    {
        _client = client;
        _model = model;
    }

    public async Task<string> GenerateTextAsync(string prompt, string apiKey)
    {
        var payload = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };

        return await _client.PostJsonAsync($"models/{_model}:generateContent", payload, apiKey);
    }
}

