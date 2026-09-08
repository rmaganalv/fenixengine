
using FenixEngine.AppCore.Src.Contracts;

namespace FenixEngine.AppCore.Src.Orchestration;


public class DecisionTreeEngine
{
    public async Task ExecuteAsync(DecisionNode? node, ServiceContext context)
    {
        await node.Handler.HandleAsync(context);

        switch (node?.Handler.State)
        {
            case HandlerState.Completed:
                foreach (var next in node.Next)
                {
                    if (next.Condition == null || next.Condition(context))
                        await ExecuteAsync(next, context);
                }
                break;

            case HandlerState.Failed:
                Console.WriteLine($"Handler {node.Handler.Name} falló. Deteniendo flujo.");
                break;

            default:
                Console.WriteLine($"Handler {node?.Handler.Name} quedó en estado {node?.Handler.State}.");
                break;
        }
    }
}
