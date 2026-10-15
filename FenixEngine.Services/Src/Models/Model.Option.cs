namespace FenixEngine.Services.Src.Models;

public class AgentOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string SelectedModel { get; set; } = string.Empty;
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2048;
    public string ApiKey { get; set; } = string.Empty; // Útil para OpenAI/Anthropic/Gemini
}
