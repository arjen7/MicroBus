namespace MicroBus.Abstractions;

public interface IMicroRequestSender
{
    Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IMicroRequest<TResponse>;
}
