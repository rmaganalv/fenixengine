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
using System.Net.Http.Headers;
using System.Text;
using FenixEngine.Agents.Utils;


namespace FenixEngine.Agents.Services.Ollama;
public class ServiceOllamaClient : IOllamaClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:11434/api";

    public ServiceOllamaClient()
    {
        _httpClient = HttpClientFactoryHelper.Instance;;
    }

    public async Task<string> PostJsonAsync(string endpoint, object payload, string? apiKey = null)
    {
        var url = $"{_baseUrl}/{endpoint}";
        var json = JsonSerializerHelper.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        if (!string.IsNullOrEmpty(apiKey))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }


    public async Task<string> GetAsync(string endpoint, string? apiKey = null)
    {
        var url = $"{_baseUrl}/{endpoint}";

        if (!string.IsNullOrEmpty(apiKey))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
