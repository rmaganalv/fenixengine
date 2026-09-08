using FenixEngine.AppCore.Src.Contracts;

namespace FenixEngine.AppCore.Src.Orchestration;


public class DecisionNode
{
    public IHandler? Handler { get; set; }
    public List<DecisionNode> Next { get; set; } = new();
    public Func<ServiceContext, bool>? Condition { get; set; }
}