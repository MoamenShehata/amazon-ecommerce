namespace Media.Application.Storage
{
    public interface IStorageService
    {
        Task<MediaFile> UploadAsync(Stream stream, bool isPublic);
    }
}