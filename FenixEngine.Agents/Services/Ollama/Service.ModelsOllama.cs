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
using System.Text.Json;

namespace FenixEngine.Agents.Services.Ollama;
public class ServiceModelsOllama
{
    private readonly IOllamaClient _client;

    public ServiceModelsOllama(IOllamaClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<string>> ListModelsAsync()
    {
        var response = await _client.GetAsync("tags");
        var models = JsonSerializer.Deserialize<IEnumerable<string>>(response);
        return models ?? Enumerable.Empty<string>();
    }
}
