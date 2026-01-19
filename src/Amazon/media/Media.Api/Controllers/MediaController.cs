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
		try
		{
            var content = await _mediaService.GetMedia(id);

            return File(content, "image/png", "fileName");
        }
		catch (Exception ex)
		{

			throw;
		}

    }
}