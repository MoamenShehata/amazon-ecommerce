using Amazon.SharedKernel.Media;
using Media.Application.Storage;
using Microsoft.Extensions.Hosting;

namespace Media.Infrastructure.Storage;

public class StorageService(IHostEnvironment _hostEnvironment) : IStorageService
{
    public async Task<MediaFile> UploadAsync(MediaContent mediaUploadRequest, bool isPublic)
    {
        var filePath = GenerateFilePath(mediaUploadRequest, isPublic);

        using (var stream = new MemoryStream(mediaUploadRequest.Content))
        using (var sw = File.OpenWrite(filePath))
        {
            await stream.CopyToAsync(sw);
            return new MediaFile(filePath, mediaUploadRequest.MimeType, mediaUploadRequest.Name, mediaUploadRequest.Content.Length);
        }
    }

    private string GenerateFilePath(MediaContent mediaUploadRequest, bool isPublic)
    {
        var directoryPath = EnsureDirectoryExists(isPublic);

        return Path.Combine(directoryPath, mediaUploadRequest.Name);
    }

    private string EnsureDirectoryExists(bool isPublic)
    {
        var filesDirectoryPath = isPublic ? "shared-files" : "private-files";
        var directoryPath = Path.Combine(_hostEnvironment.ContentRootPath, filesDirectoryPath);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        return directoryPath;
    }
}
