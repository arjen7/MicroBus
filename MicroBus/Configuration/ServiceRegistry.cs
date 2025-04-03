using MicroBus.Abstractions;

namespace MicroBus.Configuration;

public class ServiceRegistry(Dictionary<string, string> map) : IServiceRegistry
{
    private readonly Dictionary<string, string> _map = map;

    public string? GetServiceUrl(string serviceName)
    {
        return _map.TryGetValue(serviceName, out var url) ? url : null;
    }
}
