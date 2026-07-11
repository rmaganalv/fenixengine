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
using AppCore.Generator.Generators;

namespace AppCore.Generator;

public static class Orchestra
{


    private static ICodeGenerator generator = new OllamaCodeGenerator(); 
    private static string spec = "";
    private static string[] items = Array.Empty<string>();

    private static string featureName = "Calculos"; 
    private static string featureFolder = Path.Combine("Features", featureName);



        // 📄 Leer especificación
    public static async Task<States> HandleIdle()
    {
        Console.WriteLine("📄 Leyendo especificación...");
        var spec = await File.ReadAllTextAsync("spec.md");//("../../../spec.md");
        ///Users/rmagana/Documents/Local/Proyects/MCP/SandBox/bin/Debug/net10.0/spec.md
        // Parsear items desde el .md
        var items = spec.Split('\n')
                    .Where(line => line.StartsWith("-"))
                    .Select(line => line.TrimStart('-', ' '))
                    .ToArray();

        return States.Generating;
    }

    // ⚙️ Generar archivos
public static async Task<States> HandleGenerating()
{
    Console.WriteLine("⚙️ Generando feature completa...");

    Directory.CreateDirectory(featureFolder);

    string prompt = $"Genera en C# todo el código necesario para la feature {featureName}, " +
                    $"incluyendo clases, enums e interfaces. No uses explicaciones ni bloques Markdown, solo código.";

    string fullCode = await generator.GenerateCodeAsync(prompt);

    if (string.IsNullOrWhiteSpace(fullCode))
    {
        Console.WriteLine("⚠️ No se generó código.");
        return States.Error;
    }

    // Fragmentar por tipos
    var fragments = SplitByTypes(fullCode);

    int counter = 1;
    foreach (var fragment in fragments)
    {
        string fileName = $"Generated{counter}.cs";

        if (fragment.Contains("class"))
        {
            var name = ExtractName(fragment, "class");
            if (!string.IsNullOrEmpty(name)) fileName = name + ".cs";
        }
        else if (fragment.Contains("enum"))
        {
            var name = ExtractName(fragment, "enum");
            if (!string.IsNullOrEmpty(name)) fileName = name + ".cs";
        }
        else if (fragment.Contains("interface"))
        {
            var name = ExtractName(fragment, "interface");
            if (!string.IsNullOrEmpty(name)) fileName = name + ".cs";
        }

        string filePath = Path.Combine(featureFolder, fileName);
        await File.WriteAllTextAsync(filePath, fragment.Trim());
        Console.WriteLine($"✅ Archivo generado: {filePath}");
        counter++;
    }

    return States.Validating;
}

// Divide el código en fragmentos cada vez que detecta class/enum/interface
private static List<string> SplitByTypes(string code)
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

// Extrae el nombre de clase/enum/interface
private static string ExtractName(string code, string keyword)
{
    int idx = code.IndexOf(keyword);
    if (idx == -1) return null;

    var words = code.Substring(idx + keyword.Length).Trim().Split(' ');
    return words.Length > 0 ? words[0].Trim() : null;
}




    // 📄 Leer especificación
    public static async Task<States> HandleValidating()
    {
        Console.WriteLine("🔍 Validando carpeta...");

        if (!Directory.Exists(featureFolder))
        {
            Console.WriteLine("⚠️ Carpeta no encontrada.");
            return States.Error;
        }

        var files = Directory.GetFiles(featureFolder, "*.cs");
        if (files.Length == 0)
        {
            Console.WriteLine("⚠️ No se generaron archivos .cs.");
            return States.Error;
        }

        Console.WriteLine($"📂 Se generaron {files.Length} archivos en {featureFolder}.");
        return States.Completed;
    }
 

    // 🎉 Completado
    public static async Task<States> HandleCompleted()
    {
        Console.WriteLine("🎉 Feature generada completa en carpeta Features/Playlist.");
        return States.Completed;
    }

    // ❌ Error
    public static async Task<States> HandleError()
    {
        Console.WriteLine("❌ Error en la generación/validación.");
        return States.Error;
    }



}