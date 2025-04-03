namespace MicroBus.Security;

public class InternalTokenProvider(string token) : IInternalTokenProvider
{
    public string Token => token;
}
