namespace AppCore.Contracts;

public class ServiceContext
{
    // Datos de entrada/salida genéricos para los servicios
    public IDictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
    public IDictionary<string, object> Results { get; } = new Dictionary<string, object>();

    public T GetParameter<T>(string key) => Parameters.ContainsKey(key) ? (T)Parameters[key] : default!;
    public void SetResult(string key, object value) => Results[key] = value;
}

