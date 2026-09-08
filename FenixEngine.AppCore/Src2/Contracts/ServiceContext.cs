namespace FenixEngine.AppCore.Src.Contracts;


public class ServiceContext
{
    public IDictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
    public IDictionary<string, object> Results { get; } = new Dictionary<string, object>();

    public T GetParameter<T>(string key) => Parameters.ContainsKey(key) ? (T)Parameters[key] : default!;
    public void SetResult(string key, object value) => Results[key] = value;
}
