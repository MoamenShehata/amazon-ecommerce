using Amazon.SharedKernel.Media;
using Amazon.SharedKernel.Media.Events;
using Media.Application.Storage;
using Media.Domain;
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
        public async Task<MediaContent> GetMedia(Guid id)
        {
            var media = await _repository.GetInstanceAsync(id);
            if (media is null) throw new Exception();

            return new MediaContent(File.ReadAllBytes(media.Path), media.MimeType, media.Name);
        }

        public async Task<Domain.Media> CreateAsync(Guid mediaId, Guid ownerId, MediaContent mediaUploadRequest, bool isPublic)
        {
            var uploadedFile = await _storageService.UploadAsync(mediaUploadRequest, isPublic);

            var media = Create(mediaId, ownerId, uploadedFile, isPublic);

            media.RaiseEvent(new MediaCreatedEvent(media.Id, media.OwnerId, $"https://localhost:7255/files/{media.Id}"));

            _repository.Add(media);

            await _unitOfWork.CommitAsync();

            return media;
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