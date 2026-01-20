using Amazon.SharedKernel.Media;

namespace Media.Application.Storage
{
    public interface IStorageService
    {
        Task<MediaFile> UploadAsync(MediaContent mediaUploadRequest, bool isPublic);
    }
}