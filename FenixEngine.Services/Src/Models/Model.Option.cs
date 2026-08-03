namespace FenixEngine.Services.Src.Models;

public class AgentOptions
{
    public string BaseUrl { get; set; } = "http://127.0.0.1:11434/v1/";
    public string SelectedModel { get; set; } = "llama3";
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2048;
    public string ApiKey { get; set; } = string.Empty; // Útil para OpenAI/Anthropic/Gemini
}
