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
using FenixEngine.Agents.Services;

namespace Service.Agents.Services.Anthropic
{
    public class ServiceModelsAnthorpic
    {
        private readonly IAnthropicClient _client;

        public ServiceModelsAnthorpic(IAnthropicClient client)
        {
            _client = client;
        }

        public async Task<string> ListModelsAsync(string apiKey)
        {
            return await _client.GetAsync("models", apiKey);
        }
    }
}
