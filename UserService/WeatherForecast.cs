using MicroBus.Abstractions;
using Shared;

namespace UserService
{
    public class GetUserByIdQueryHandler : IMicroHandler<GetUserByIdQuery, UserDto>
    {
        public Task<UserDto> HandleAsync(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new UserDto
            {
                Id = request.UserId,
                Email = "john.doe@example.com"
            });
        }
    }

    
}
