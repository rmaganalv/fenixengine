using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

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
