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
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace FenixEngine.Agents.Services.Master;

public class OnnxPredictor
{
    private readonly InferenceSession _session;

    public OnnxPredictor(string modelPath)
    {
        // Carga el archivo .onnx en memoria
        _session = new InferenceSession(modelPath);
    }

    public float[] Predict(float[] inputData)
    {
        // 1. Definir las dimensiones de entrada que espera el modelo (ejemplo: batch 1, 10 features)
        var inputTensor = new DenseTensor<float>(inputData, new[] { 1, inputData.Length });

        // 2. Empaquetar la entrada con el nombre del nodo que espera el modelo
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_node_name", inputTensor)
        };

        // 3. Ejecutar la inferencia directo en el hardware
        using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = _session.Run(inputs);

        // 4. Extraer el resultado
        var outputTensor = results.First().AsTensor<float>();
        return outputTensor.ToArray();
    }
}
