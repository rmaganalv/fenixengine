using System.Text;

namespace AppCore.Generator.Generators;

public class OllamaCodeGenerator : ICodeGenerator
{
    /// <summary>
    /// HttpClient utilizado para realizar solicitudes HTTP al servicio Ollama.
    /// </summary>
    private readonly HttpClient _client = new HttpClient();
    /// <summary>
    /// URL del endpoint de la API de Ollama para generar código.
    /// </summary>
    private readonly string? _api = "http://localhost:11434/api/generate";
    

    /// <summary>
    /// Genera código a partir de un prompt utilizando el servicio Ollama. 
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    public async Task<string> GenerateCodeAsync(string prompt)
    {
        var payload = new { model = "codegemma", prompt = prompt };
        var json = System.Text.Json.JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(_api, content);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        var sb = new StringBuilder();
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var obj = System.Text.Json.JsonDocument.Parse(line);
            if (obj.RootElement.TryGetProperty("response", out var resp))
            {
                sb.Append(resp.GetString());
            }
        }
        var generatedCode = sb.ToString();
        return Sanitizant.Sanitize(generatedCode);
    }
}