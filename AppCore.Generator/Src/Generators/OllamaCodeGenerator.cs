// Copyright (C) 2026 Ruben Magaña Alvarado
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
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