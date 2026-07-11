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
namespace AppCore.Generator.Generators;

public static class Sanitizant
{
    public static string Sanitize(string input)
    {
        // Reemplaza caracteres especiales por sus equivalentes seguros
        var sanitized = 
                    input.Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("&", "&amp;")
                    .Replace("\"", "&quot;")
                    .Replace("'", "&#39;")
                    .Replace("```csharp", "")
                    .Replace("```C#", "")
                    .Replace("```cs", "")
                    .Replace("```", "")
                    .Trim();
        return sanitized;
    }

    // Método auxiliar para extraer nombre de clase/enum
    public static string ExtractName(string code, string keyword)
    {
        int idx = code.IndexOf(keyword);
        if (idx == -1) return null;

        var words = code.Substring(idx + keyword.Length).Trim().Split(' ');
        return words?.Length > 0 ? words[0].Trim() : null;
    }
}