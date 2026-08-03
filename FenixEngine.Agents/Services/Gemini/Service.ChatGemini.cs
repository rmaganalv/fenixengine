namespace FenixEngine.Agents.Services.Gemini;

public class ServiceChatGemini
{
    private readonly IGeminiClient _client;
    private readonly string _model;

    public ServiceChatGemini(IGeminiClient client, string model = "gemini-pro")
    {
        _client = client;
        _model = model;
    }

    public async Task<string> SendMessageAsync(IEnumerable<string> messages, string apiKey)
    {
        var payload = new
        {
            contents = messages.Select(m => new { parts = new[] { new { text = m } } }).ToArray()
        };

        return await _client.PostJsonAsync($"models/{_model}:generateContent", payload, apiKey);
    }
}

