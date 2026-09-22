using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace FenixEngine.Services.Models;

public class AIServiceOptions : INotifyPropertyChanged
{
    private string _baseUrl = "http://127.0.0.1:8080/v1/";
    private string _selectedModel = string.Empty;
    private double _temperature = 0.2;
    private int _maxTokens = -1;

    public string BaseUrl
    {
        get => _baseUrl;
        set { _baseUrl = value; OnPropertyChanged(); }
    }

    public string SelectedModel
    {
        get => _selectedModel;
        set { _selectedModel = value; OnPropertyChanged(); }
    }

    public double Temperature
    {
        get => _temperature;
        set { _temperature = value; OnPropertyChanged(); }
    }

    public int MaxTokens
    {
        get => _maxTokens;
        set { _maxTokens = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

// Modelos para la llamada de chat y lista de modelos HTTP
public record ChatMessage([property: JsonPropertyName("role")] string Role, [property: JsonPropertyName("content")] string Content);
public record ChatCompletionRequest([property: JsonPropertyName("model")] string Model, [property: JsonPropertyName("messages")] List<ChatMessage> Messages, [property: JsonPropertyName("temperature")] double Temperature, [property: JsonPropertyName("max_tokens")] int MaxTokens);
public record ChatCompletionResponse([property: JsonPropertyName("choices")] List<ChatChoice> Choices);
public record ChatChoice([property: JsonPropertyName("message")] ChatMessage Message);

public record ModelData([property: JsonPropertyName("id")] string Id);
public record ModelListResponse([property: JsonPropertyName("data")] List<ModelData> Data);
