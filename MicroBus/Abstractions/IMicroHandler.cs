namespace MicroBus.Abstractions;

public interface IMicroHandler<in TRequest, TResponse>
    where TRequest : IMicroRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
