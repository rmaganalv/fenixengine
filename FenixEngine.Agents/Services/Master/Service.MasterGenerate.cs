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
using Microsoft.ML.OnnxRuntimeGenAI;

namespace FenixEngine.Agents.Services.Master;

public class ServiceMasterGenerate
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
