using MicroBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace OrderService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IMicroRequestSender sender) : ControllerBase
{
    private readonly IMicroRequestSender _sender = sender;

    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        var user = await _sender.SendAsync<GetUserByIdQuery, UserDto>(new GetUserByIdQuery { UserId = 14 });
        return Ok(user);
    }
}
