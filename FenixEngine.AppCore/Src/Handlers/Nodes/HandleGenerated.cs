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
using Service.Resources.Contracts;

using Service.Resources.Agentics;
using Service.Resources.SystemFiles;
using FenixEngine.AppCore.Utils.Messages;
using FenixEngine.AppCore.src.Utils.Messages;
using FenixEngine.AppCore.Utils.Enums;
using FenixEngine.AppCore.Utils.Generators;
using FenixEngine.Service.Agentics;
namespace FenixEngine.AppCore.Executions;

/// <summary>
/// 
/// </summary>
public static class HandleGeneratingExecution
{   
    private static ICodeGenerator generator = new LocalCodeGenerator(); 
    private static IMessageService messageService = new MessagesService();
    private static ISystemFiles systemFiles = new SystemFiles(); 
    private static string featureName = "Calculos";
    private static string featureFolder = Path.Combine("Features", featureName);
    // ⚙️ Generar archivos
    public static async Task<StatesGenerator> HandleGenerating()
    {
        //systemFiles.PickFile();
        string selectedFile = await systemFiles.PickFile(Environment.CurrentDirectory);
        if (!string.IsNullOrEmpty(selectedFile))
        {
            Console.WriteLine($"Archivo seleccionado: {selectedFile}");
        }
        
        
        
        HandleExtension.HandleStart(messageService.GetMessage("Start"));

        Directory.CreateDirectory(featureFolder);

        string prompt = featureName;// $"Genera en C# todo el código necesario para la feature {featureName}, " +
                        //$"incluyendo clases, enums e interfaces. No uses explicaciones ni bloques Markdown, solo código.";

        var generatedCode = await generator.GenerateCodeAsync(prompt);
        string fullCode = Sanitizant.Sanitize(generatedCode);

        if (string.IsNullOrWhiteSpace(fullCode))
        {
            return HandleExtension.HandleError(messageService.GetErrorMessage());
        }

        // Fragmentar por tipos
        var fragments = Sanitizant.SplitByTypes(fullCode);

        int counter = 1;
        foreach (var fragment in fragments)
        {
            string fileName = $"Generated{counter}.cs";

            if (fragment.Contains("class"))
            {
                var name = Sanitizant.ExtractName(fragment, "class");
                if (!string.IsNullOrEmpty(name)) fileName = name + ".cs";
            }
            else if (fragment.Contains("enum"))
            {
                var name = Sanitizant.ExtractName(fragment, "enum");
                if (!string.IsNullOrEmpty(name)) fileName = name + ".cs";
            }
            else if (fragment.Contains("interface"))
            {
                var name = Sanitizant.ExtractName(fragment, "interface");
                if (!string.IsNullOrEmpty(name)) fileName = name + ".cs";
            }

            string filePath = Path.Combine(featureFolder, fileName);
            await File.WriteAllTextAsync(filePath, fragment.Trim());
            HandleExtension.HandleStart(messageService.GetMessage("Generate File") + filePath);
            counter++;
        }
        
        return HandleExtension.HandleFinish(messageService.GetMessage("Finish"));
    }
}