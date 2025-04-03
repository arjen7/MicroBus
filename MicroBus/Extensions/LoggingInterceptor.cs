using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace MicroBus.Extensions;

public class LoggingInterceptor(ILogger<LoggingInterceptor> logger) : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger = logger;

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var requestName = typeof(TRequest).Name;
        var start = DateTime.UtcNow;

        _logger.LogInformation("➡️ Request received: {Request}", requestName);

        try
        {
            var response = await continuation(request, context);
            _logger.LogInformation("✅ Request handled: {Request} in {Elapsed}ms", requestName, (DateTime.UtcNow - start).TotalMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error handling request: {Request}", requestName);
            throw;
        }
    }
}
