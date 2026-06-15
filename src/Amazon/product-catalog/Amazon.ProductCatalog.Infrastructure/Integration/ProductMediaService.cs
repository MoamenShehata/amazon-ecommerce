using Amazon.ProductCatalog.Domain.Products;
using Amazon.SharedKernel.Media;
using DnsClient.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Amazon.ProductCatalog.Infrastructure.Integration;

internal class UploadFileResult
{
    public string Url { get; set; }
}

public class ProductMediaService(
    ILogger<ProductMediaService> _logger,
    MediaRestClient _mediaRestClient,
    IConfiguration _configuration
    ) : IMediaService
{
    public async Task<string> UploadFileAsync(MediaContent uploadRequest)
    {
        try
        {
            _logger.LogDebug("Uploading product image {imageName}", uploadRequest.Name);

            var uploadedFileUrl = await _mediaRestClient.UploadFileAsync(new UploadFileRequest(Guid.NewGuid(), true, uploadRequest.Content, uploadRequest.MimeType, uploadRequest.Name));
            return uploadedFileUrl;
        }
        catch (Exception ex)
        {
            return string.Empty;
            _logger.LogError(ex, "Failed to upload product image {imageName}", uploadRequest.Name);
        }

    }
}