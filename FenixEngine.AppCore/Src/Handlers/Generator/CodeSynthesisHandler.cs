using FenixEngine.AppCore.Utils.Generators;
using FenixEngine.Service.Agentics;
using Service.Resources.Agentics;
using Service.Resources.Contracts;

namespace AppCore.Generator.Nodes.Common;

public class CodeSynthesisHandler
{
    private readonly ICodeGenerator _generator;

    public CodeSynthesisHandler(ICodeGenerator? generator = null)
    {
        _generator = generator ?? new LocalCodeGenerator();
    }

    public async Task<string> SynthesizeCodeAsync(string prompt)
    {
        var rawCode = await _generator.GenerateCodeAsync(prompt);
        string sanitizedCode = Sanitizant.Sanitize(rawCode);

        if (string.IsNullOrWhiteSpace(sanitizedCode))
        {
            throw new InvalidOperationException("El código generado por la IA está vacío o no es válido.");
        }

        return sanitizedCode;
    }



}