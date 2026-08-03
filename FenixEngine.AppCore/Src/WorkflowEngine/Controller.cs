using FenixEngine.AppCore.Src.Nodes;
using FenixEngine.AppCore.Utils;

namespace MiProyectoMaui.Services;

public class PipelineOrchestrator
{
    private readonly Dictionary<NodeId, IPiezaSoftware<NodeId>> _piezas;
    private readonly DecisionTreeService _treeService;

    public PipelineOrchestrator(
        IEnumerable<IPiezaSoftware<NodeId>> piezas, 
        DecisionTreeService treeService)
    {
        _piezas = piezas.ToDictionary(p => p.Id);
        _treeService = treeService;
    }

    public async Task<ContextoEjecucion> EjecutarCasoUsoAsync(string casoDeUso, Action<ContextoEjecucion>? setupContext = null)
    {
        var ctx = new ContextoEjecucion();
        
        // Permite inyectar variables iniciales si se requiere
        setupContext?.Invoke(ctx);

        List<NodeId> secuencia = _treeService.ObtenerSecuencia(casoDeUso);

        foreach (NodeId idPieza in secuencia)
        {
            if (_piezas.TryGetValue(idPieza, out var pieza))
            {
                ctx.Log($"---> Ejecutando {idPieza}");
                await pieza.EjecutarAsync(ctx);

                if (ctx.Cancelado)
                {
                    ctx.Log($"Pipeline cancelado en {idPieza}. Motivo: {ctx.MotivoCancelacion}");
                    break;
                }
            }
            else
            {
                throw new InvalidOperationException($"La pieza {idPieza} no está registrada en el contenedor de dependencias.");
            }
        }

        return ctx;
    }
}
