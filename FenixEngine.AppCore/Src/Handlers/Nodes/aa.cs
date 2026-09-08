
/*using FenixEngine.AppCore.Utils.Contracts;
using FenixEngine.AppCore.Utils.Messages;
using FenixEngine.AppCore.src.Utils.Messages;
using FenixEngine.AppCore.Utils.Enums;
using FenixEngine.AppCore.Utils.Generators;
using Service.Resources.Contracts;
using Service.Resources.Agentics;
using Service.Resources.SystemFiles;

namespace FenixEngine.AppCore.Executions;

public class CodeGenerationExecution : NodeBase<StatesGenerator>
{
    private readonly ICodeGenerator _generator = new LocalCodeGenerator(); 
    private readonly IMessageService _messageService = new MessagesService();
    private readonly ISystemFiles _systemFiles = new SystemFiles();

    protected override async Task<StatesGenerator> OnBeforeExecuteAsync(string featureName)
    {
        string selectedFile = await _systemFiles.PickFile(Environment.CurrentDirectory);
        if (!string.IsNullOrEmpty(selectedFile))
        {
            LogStep($"Archivo seleccionado correctamente: {selectedFile}");
        }
        return StatesGenerator.Error;
    }

    protected override async Task<StatesGenerator> OnExecuteAsync(string featureName)
    {
        string featureFolder = Path.Combine("Features", featureName);
        Directory.CreateDirectory(featureFolder);

        string prompt = featureName;
        var generatedCode = await _generator.GenerateCodeAsync(prompt);
        string fullCode = Sanitizant.Sanitize(generatedCode);

        if (string.IsNullOrWhiteSpace(fullCode))
        {
            throw new InvalidOperationException(_messageService.GetErrorMessage());
        }

        var fragments = Sanitizant.SplitByTypes(fullCode);
        int counter = 1;

        foreach (var fragment in fragments)
        {
            string fileName = "";//ResolveFileName(fragment, counter);
            string filePath = Path.Combine(featureFolder, fileName);

            await File.WriteAllTextAsync(filePath, fragment.Trim());
            LogStep($"{_messageService.GetMessage("Generate File")}: {filePath}");
            counter++;
        }

        return HandleExtension.HandleFinish(_messageService.GetMessage("Finish"));
    }

    protected override async Task<StatesGenerator> OnError(Exception exception)
    {
        return HandleExtension.HandleError(exception.Message);
    }

    /*
    private static string ResolveFileName(string fragment, int defaultIndex)
    {
        string fileName = $"Generated{defaultIndex}.cs";

        if (fragment.Contains("class"))
        {
            var name = Sanitizant.ExtractName(fragment, "class");
            if (!string.IsNullOrEmpty(name)) fileName = $"{name}.cs";
        }
        else if (fragment.Contains("enum"))
        {
            var name = Sanitizant.ExtractName(fragment, "enum");
            if (!string.IsNullOrEmpty(name)) fileName = $"{name}.cs";
        }
        else if (fragment.Contains("interface"))
        {
            var name = Sanitizant.ExtractName(fragment, "interface");
            if (!string.IsNullOrEmpty(name)) fileName = $"{name}.cs";
        }

        return fileName;
    }
}*/