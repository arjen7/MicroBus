using Grpc.Core;
using Grpc.Core.Interceptors;

namespace MicroBus.Security;

public class InternalAuthInterceptor(string expectedToken) : Interceptor
{
    private readonly string _expectedToken = expectedToken;

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var headers = context.RequestHeaders;
        var token = headers.GetValue("x-internal-token");

        if (token != _expectedToken)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid internal token."));
        }

        return await continuation(request, context);
    }
}

public static class MetadataExtensions
{
    public static string? GetValue(this Metadata headers, string key)
    {
        return headers.FirstOrDefault(h => h.Key.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
