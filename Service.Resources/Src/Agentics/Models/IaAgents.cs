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
namespace Service.Resources.Agentics.Models;

public class IaAgents
{
    private string? Model { get; set; }//"codegemma:7b"
    private string? Url { get; set; } //"http://localhost:11434/api/generate"
    private string? Prompt { get; set; }


    public IaAgents(string modelAgent, string urlAgent, string promptAgent)
    {
        Model = modelAgent;
        Url = urlAgent;
        Prompt = promptAgent;
    }

    public string GetModel()
    {
        return Model?.ToString() ?? string.Empty;
    }

    public string GetUrl()
    {
        return Url?.ToString() ?? string.Empty;
    }

    public string GetPrompt()
    {
        return Prompt ?? string.Empty;
    }


    
}