namespace Media.Api.Dtos;

public record UploadMediaRequest(Guid OwnerId, bool IsPublic, byte[] Content, string MimeType, string Name) { }