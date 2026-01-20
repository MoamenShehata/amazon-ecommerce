using Media.Application;
using Microsoft.AspNetCore.Mvc;

namespace Media.Api.Controllers;

[ApiController]
[Route("files")]
public class MediaController(MediaService _mediaService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult> GetMedia(Guid id)
    {
        var media = await _mediaService.GetMedia(id);

        return File(media.Content, media.MimeType, media.Name);
    }
}