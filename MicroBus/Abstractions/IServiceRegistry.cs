namespace MicroBus.Abstractions;

public interface IServiceRegistry
{
    string? GetServiceUrl(string serviceName);
}
