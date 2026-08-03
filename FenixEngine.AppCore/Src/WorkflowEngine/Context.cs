// KeyContext.cs
public static class Keys
{
    public const string InputCasoUso = "Input_CasoUso";
    public const string RutaSalida = "Output_RutaSalida";
    public const string ProyectoCreado = "Model_Proyecto";
}

// ContextoEjecucion.cs
public class ContextoEjecucion
{
    private readonly Dictionary<string, object> _datos = new();

    public bool Cancelado { private set; get; }
    public string? MotivoCancelacion { private set; get; }
    public List<string> Bitacora { get; } = new();

    public void Set<T>(string clave, T valor) => _datos[clave] = valor!;

    public T Get<T>(string clave)
    {
        if (_datos.TryGetValue(clave, out var valor) && valor is T valorTipado)
            return valorTipado;

        throw new KeyNotFoundException($"La clave '{clave}' no existe en el contexto.");
    }

    public bool TryGet<T>(string clave, out T? valor)
    {
        if (_datos.TryGetValue(clave, out var valorObj) && valorObj is T valorTipado)
        {
            valor = valorTipado;
            return true;
        }
        valor = default;
        return false;
    }

    public void Cancelar(string motivo)
    {
        Cancelado = true;
        MotivoCancelacion = motivo;
    }

    public void Log(string msj) => Bitacora.Add($"[{DateTime.Now:HH:mm:ss}] {msj}");
}

// IPiezaSoftware.cs
public interface IPiezaSoftware
{
    int Id { get; } // Identificador único de la pieza (1, 2, 7, 34, etc.)
    Task EjecutarAsync(ContextoEjecucion ctx);
}
