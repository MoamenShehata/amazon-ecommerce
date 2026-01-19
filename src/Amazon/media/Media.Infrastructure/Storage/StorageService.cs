using Media.Application.Storage;
using Microsoft.Extensions.Hosting;
using MimeDetective;

namespace Media.Infrastructure.Storage;

public class StorageService(IHostEnvironment _hostEnvironment) : IStorageService
{
    public async Task<MediaFile> UploadAsync(byte[] content, bool isPublic)
    {
        var filePath = GenerateRandomFilePath(isPublic);

        var inspector = new ContentInspectorBuilder().Build();

        //var result = inspector.Inspect(content);
        //var mimeType = result.ByMimeType().FirstOrDefault().MimeType;

        using (var stream = new MemoryStream(content))
        using (var sw = File.OpenWrite(filePath))
        {
            await stream.CopyToAsync(sw);
            return new MediaFile(filePath, "mimeType", Path.GetFileName(filePath), content.Length);
        }
    }

    private string GenerateRandomFilePath(bool isPublic)
    {
        var filesDirectoryPath = isPublic ? "shared-files" : "private-files";
        var directoryPath = Path.Combine(_hostEnvironment.ContentRootPath, filesDirectoryPath);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        return Path.Combine(_hostEnvironment.ContentRootPath, filesDirectoryPath, Guid.NewGuid().ToString());
    }
}
