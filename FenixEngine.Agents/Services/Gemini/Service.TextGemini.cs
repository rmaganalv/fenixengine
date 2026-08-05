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

