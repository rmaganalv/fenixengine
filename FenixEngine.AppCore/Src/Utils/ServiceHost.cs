using System.Collections.Concurrent;
using AppCore.Contracts;

namespace AppCore.Orchestration;

public class ServiceHost
{
    private readonly ConcurrentDictionary<string, IAppService> _services = new();

    public void Register(IAppService service)
    {
        _services[service.Name] = service;
    }

    public IAppService? GetService(string name)
    {
        return _services.TryGetValue(name, out var svc) ? svc : null;
    }

    public async Task ExecuteAsync(string serviceName, ServiceContext context)
    {
        var service = GetService(serviceName);
        if (service == null)
            throw new InvalidOperationException($"Service '{serviceName}' not found.");

        await service.ExecuteAsync(context);
    }

    public IEnumerable<IAppService> GetServicesByCategory(string category)
    {
        return _services.Values.Where(s => s.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
    }
}

