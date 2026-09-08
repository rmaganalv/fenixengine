

namespace FenixEngine.AppCore.Src.Nodes;

public interface IPiezaSoftware<TEnum> where TEnum : Enum
{
    TEnum Id { get; }
    Task EjecutarAsync(ContextoEjecucion ctx);
}
