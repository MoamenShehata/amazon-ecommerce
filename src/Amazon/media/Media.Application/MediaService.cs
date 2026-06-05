using Amazon.SharedKernel.Media;
using Amazon.SharedKernel.Media.Events;
using Media.Application.Storage;
using Media.Domain;
using Media.Domain.Factories;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using StackExchange.Redis;

namespace Media.Application
{
    public class MediaService(
        MediaFactory _factory,
        IStorageService _storageService,
        IRepository<Domain.Media, Guid> _repository,
        IUnitOfWork _unitOfWork,
        IConnectionMultiplexer _redis)
    {
        private readonly IDatabase _db = _redis.GetDatabase();
        public async Task<MediaContent> GetMedia(Guid id)
        {
            var mediaFromCache = _db.HashGetAll($"{id}");
            if (mediaFromCache.Length == 0)
            {
                var media = await _repository.GetInstanceAsync(id);
                if (media is null) throw new Exception();
                _db.HashSet($"{id}", [new HashEntry("Path", media.Path), new HashEntry("MimeType", media.MimeType), new HashEntry("Name", media.Name)]);
                return new MediaContent(File.ReadAllBytes(media.Path), media.MimeType, media.Name);
            }

            return new MediaContent(File.ReadAllBytes(mediaFromCache.FirstOrDefault(x => x.Name == "Path").Value), mediaFromCache.FirstOrDefault(x => x.Name == "MimeType").Value, mediaFromCache.FirstOrDefault(x => x.Name == "Name").Value);
        }

        public async Task<Domain.Media> CreateAsync(Guid mediaId, Guid ownerId, MediaContent mediaUploadRequest, bool isPublic)
        {
            var uploadedFile = await _storageService.UploadAsync(mediaUploadRequest, isPublic);

            var media = Create(mediaId, ownerId, uploadedFile, isPublic);

            media.RaiseEvent(new MediaCreatedEvent(media.Id, media.OwnerId, $"http://localhost:5104/files/{media.Id}"));

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