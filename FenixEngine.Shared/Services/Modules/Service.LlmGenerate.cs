using System;
using Microsoft.ML.OnnxRuntimeGenAI;

public class LocalLlmEngine
{
    public void GenerateResponse(string modelFolderPath, string prompt)
    {
        // Carga la carpeta que contiene el modelo ONNX y sus configuraciones
        using var model = new Model(modelFolderPath);
        using var tokenizer = new Tokenizer(model);

        // Formatear el prompt según el modelo
        var fullPrompt = $"<|user|>\n{prompt}<|end|>\n<|assistant|>";
        var sequences = tokenizer.Encode(fullPrompt);

        using var generatorParams = new GeneratorParams(model);
        generatorParams.SetSearchOption("max_length", 512);
        generatorParams.SetInputSequences(sequences);

        using var generator = new Generator(model, generatorParams);

        // Generar respuesta token por token (Streaming en consola o UI)
        while (!generator.IsDone())
        {
            generator.ComputeLogits();
            generator.GenerateNextToken();

            var outputSequence = generator.GetSequence(0);
            var newToken = outputSequence[^1];
            
            // Imprime el fragmento generado en tiempo real
            Console.Write(tokenizer.Decode(new[] { newToken }));
        }
    }
}
