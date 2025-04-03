using MicroBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBus.Transport.Dispatcher;

public class MicroHandlerRegistry(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public bool TryGetHandler<TRequest, TResponse>(out IMicroHandler<TRequest, TResponse>? handler)
        where TRequest : IMicroRequest<TResponse>
    {
        handler = _serviceProvider.GetService<IMicroHandler<TRequest, TResponse>>();
        return handler is not null;
    }
}
