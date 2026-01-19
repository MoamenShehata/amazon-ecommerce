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
        public async Task CreateAsync(Guid mediaId, Guid ownerId, byte[] stream, bool isPublic)
        {
            var uploadedFile = await _storageService.UploadAsync(stream, isPublic);

            var media = Create(mediaId, ownerId, uploadedFile, isPublic);

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