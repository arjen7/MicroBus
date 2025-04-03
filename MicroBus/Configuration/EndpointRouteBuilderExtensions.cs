using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using MicroBus.Transport.Grpc;

namespace MicroBus.Configuration;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder UseMicroBusEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGrpcService<GenericGrpcService>();
        return app;
    }
}
