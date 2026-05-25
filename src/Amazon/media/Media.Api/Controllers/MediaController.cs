using Amazon.SharedKernel.Media;
using Media.Api.Dtos;
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

    [HttpPost]
    public async Task<ActionResult> UploadMedia([FromBody] UploadMediaRequest request)
    {
        var media = await _mediaService.CreateAsync(Guid.NewGuid(), request.OwnerId, new MediaContent(request.Content, request.MimeType, request.Name), request.IsPublic);

        return Ok(new { Url = $"https://localhost:7255/files/{media.Id}" });
    }
}