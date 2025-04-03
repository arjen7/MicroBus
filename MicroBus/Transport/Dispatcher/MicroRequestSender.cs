using Grpc.Net.Client;
using MicroBus.Abstractions;
using MicroBus.Transport.Grpc;
using System.Reflection;

namespace MicroBus.Transport.Dispatcher
{
    public class MicroRequestSender(
        MicroHandlerRegistry handlerRegistry,
        IServiceRegistry serviceRegistry,
        IServiceProvider serviceProvider) : IMicroRequestSender
    {
        private readonly MicroHandlerRegistry _handlerRegistry = handlerRegistry;
        private readonly IServiceRegistry _serviceRegistry = serviceRegistry;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IMicroRequest<TResponse>
        {
            var routeToAttr = typeof(TRequest).GetCustomAttribute<RouteToAttribute>()
                              ?? throw new InvalidOperationException($"Request of type {typeof(TRequest).Name} must have [RouteTo] attribute.");

            var serviceUrl = _serviceRegistry.GetServiceUrl(routeToAttr.ServiceName)
                             ?? throw new InvalidOperationException($"Service '{routeToAttr.ServiceName}' not registered.");

            using var channel = GrpcChannel.ForAddress(serviceUrl);
            var client = new GenericGrpcClient(channel, _serviceProvider);

            return await client.SendAsync<TRequest, TResponse>(request, cancellationToken);
        }
    }
}