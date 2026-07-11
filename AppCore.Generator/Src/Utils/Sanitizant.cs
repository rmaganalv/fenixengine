
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