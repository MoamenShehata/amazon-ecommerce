using Amazon.SharedKernel.Media.Events;
using Media.Application.Storage;
using Media.Domain.Factories;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Media.Application
{
    public class MediaService(
        MediaFactory _factory,
        IStorageService _storageService,
        IRepository<Domain.Media, Guid> _repository,
        IUnitOfWork _unitOfWork)
    {
        public async Task<byte[]> GetMedia(Guid id)
        {
            var media = await _repository.GetInstanceAsync(id);
            if (media is null) throw new Exception();

            return File.ReadAllBytes(media.Path);
        }

        public async Task CreateAsync(Guid mediaId, Guid ownerId, byte[] stream, bool isPublic)
        {
            var uploadedFile = await _storageService.UploadAsync(stream, isPublic);

            var media = Create(mediaId, ownerId, uploadedFile, isPublic);

            media.RaiseEvent(new MediaCreatedEvent(media.Id, media.OwnerId, $"https://localhost:7255/files/{media.Id}"));

            _repository.Add(media);

            await _unitOfWork.CommitAsync();
        }

        private Domain.Media Create(Guid mediaId, Guid ownerId, MediaFile mediaFile, bool isPublic)
        {
            Func<Guid, Guid, long, string, string, string, Domain.Media> factory =
                isPublic ? _factory.CreateForPublic
                : _factory.CreateSecured;

            return factory(mediaId, ownerId, mediaFile.SizeInBytes, mediaFile.Name, mediaFile.Path, mediaFile.MimeType);
        }
    }
}