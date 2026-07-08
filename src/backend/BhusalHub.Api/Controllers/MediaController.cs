using BhusalHub.Core.Interfaces;
using BhusalHub.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BhusalHub.Api.Controllers;

[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Media>>> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 30)
    {
        var items = await _mediaService.ListAsync(page, pageSize);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Media>> Get(string id)
    {
        var media = await _mediaService.GetByIdAsync(id);
        if (media == null) return NotFound();
        return Ok(media);
    }
}
