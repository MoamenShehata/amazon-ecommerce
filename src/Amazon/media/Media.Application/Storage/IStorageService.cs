namespace Media.Application.Storage
{
    public interface IStorageService
    {
        Task<MediaFile> UploadAsync(byte[] stream, bool isPublic);
    }
}