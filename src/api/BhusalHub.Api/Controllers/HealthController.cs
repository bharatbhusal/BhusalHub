using BhusalHub.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BhusalHub.Api.Controllers;

[ApiController]
[Route("api")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet("health")]
    public IActionResult Get()
    {
        var result = _healthService.Check();
        return result.Database == "connected"
            ? Ok(result)
            : StatusCode(503, result);
    }
}
