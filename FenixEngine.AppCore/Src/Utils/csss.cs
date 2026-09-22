/*using AppCore.Contracts;

public class ConditionalNode
{
    public IHandler Handler { get; set; }
    public Func<ServiceContext, bool>? Condition { get; set; }
    public List<ConditionalNode> Next { get; set; } = new();
}

public class ConditionalEngine
{
    public async Task ExecuteAsync(ConditionalNode node, ServiceContext context)
    {
        await node.Handler.HandleAsync(context);

        if (node.Handler.State == HandlerState.Completed)
        {
            foreach (var next in node.Next)
            {
                if (next.Condition == null || next.Condition(context))
                    await ExecuteAsync(next, context);
            }
        }
        else
        {
            Console.WriteLine($"Nodo {node.Handler.Name} falló. Estado: {node.Handler.State}");
        }
    }
}*/
