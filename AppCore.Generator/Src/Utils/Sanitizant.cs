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
using System.Text.RegularExpressions;

namespace AppCore.Generator.Generators;

public static class Sanitizant
{
    public static string Sanitize(string input)
    {
        // Reemplaza caracteres especiales por sus equivalentes seguros
        var sanitized = 
                    input
                    //.Replace("<", "&lt;")
                    //.Replace(">", "&gt;")
                    //.Replace("&", "&amp;")
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
    public static string? ExtractName(string code, string keyword)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(keyword))
            return null;

        var pattern = $@"\b{keyword}\s+([A-Za-z_][A-Za-z0-9_]*)";
        var match = Regex.Match(code, pattern);

        return match.Success ? match.Groups[1].Value : null;
    }


    // Divide el código en fragmentos cada vez que detecta class/enum/interface
    public static List<string> SplitByTypes(string code)
    {
        var fragments = new List<string>();
        var lines = code.Split('\n');

        var sb = new StringBuilder();
        foreach (var line in lines)
        {
            if (line.Contains("class") || line.Contains("enum") || line.Contains("interface"))
            {
                if (sb.Length > 0)
                {
                    fragments.Add(sb.ToString());
                    sb.Clear();
                }
            }
            sb.AppendLine(line);
        }

        if (sb.Length > 0)
            fragments.Add(sb.ToString());

        return fragments;
    }

}