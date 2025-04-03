namespace MicroBus.Abstractions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class RouteToAttribute(string serviceName) : Attribute
{
    public string ServiceName { get; } = serviceName;
}
