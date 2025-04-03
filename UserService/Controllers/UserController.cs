using MicroBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UserService.Controllers;
[ApiController]
[Route("api/users")]
public class UsersController(IMicroRequestSender sender) : ControllerBase
{
    [HttpPost("order")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var result = await sender.SendAsync<CreateOrderCommand, string>(command);
        return Ok(result);
    }
}

