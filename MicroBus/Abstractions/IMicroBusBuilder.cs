namespace MicroBus.Abstractions
{
    public interface IMicroBusBuilder
    {
        IMicroBusBuilder WithService(string serviceName, string url);
        IMicroBusBuilder UseInternalToken(string token);
        IMicroBusBuilder UseLogging();
    }
}
