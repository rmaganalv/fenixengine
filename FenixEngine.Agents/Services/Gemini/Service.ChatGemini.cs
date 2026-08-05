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

