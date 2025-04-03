using MicroBus.Abstractions;

namespace Shared
{
    [RouteTo("UserService")]
    public class GetUserByIdQuery : IMicroRequest<UserDto>
    {
        public int UserId { get; set; }
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
    }

    [RouteTo("OrderService")]
    public class CreateOrderCommand : IMicroRequest<string>
    {
        public int UserId { get; set; }
        public string Product { get; set; } = default!;
    }
    
}
