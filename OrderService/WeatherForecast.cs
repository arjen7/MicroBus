using MicroBus.Abstractions;
using Shared;

namespace OrderService
{

    public class CreateOrderCommandHandler : IMicroHandler<CreateOrderCommand, string>
    {
        public Task<string> HandleAsync(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Order created for {request.UserId} - {request.Product}");
        }
    }


}