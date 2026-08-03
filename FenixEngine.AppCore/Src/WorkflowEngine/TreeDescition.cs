using FenixEngine.AppCore.Utils;

namespace MiProyectoMaui.Services;

public class DecisionTreeService
{
    public List<NodeId> ObtenerSecuencia(string casoDeUso)
    {
        return casoDeUso switch
        {
            "CrearProyecto" => new List<NodeId> 
            { 
                NodeId.ValidarEntornos, 
                NodeId.CrearEstructura, 
                NodeId.GenerarDocumentacion 
            },

            "GenerarDocumentacion" => new List<NodeId> 
            { 
                NodeId.ValidarEntornos, 
                NodeId.GenerarDocumentacion 
            },

            _ => throw new ArgumentException($"Caso de uso no reconocido: '{casoDeUso}'")
        };
    }
}
