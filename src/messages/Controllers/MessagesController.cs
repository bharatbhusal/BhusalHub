using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messages.Api.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessagesController : ControllerBase
{
    [HttpGet("secret")]
    public IActionResult GetSecret()
    {
        return Ok(new { message = "Your secret message: Keep building awesome things!" });
    }
}
