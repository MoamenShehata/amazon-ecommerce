using Amazon.SharedKernel.Media;

namespace Amazon.ProductCatalog.Domain.Products;

public interface IMediaService
{
    Task<string> UploadFileAsync(MediaContent uploadRequest);
}