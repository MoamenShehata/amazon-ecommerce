using Media.Domain.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;
using Amazon.SharedKernel.Media.Events;

namespace Media.Domain
{
    public class Media : AuditableAggregate<Guid>, IEntity<Guid>
    {
        public Guid OwnerId { get; private set; }
        public long SizeInBytes { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string MimeType { get; private set; }

        public MediaAccessibility Accessibility { get; set; }

        internal Media(Guid id, Guid ownerId, long sizeInBytes, string name, string path, string mimeType, MediaAccessibility accessibility) : base(id)
        {
            OwnerId = ownerId;
            SizeInBytes = sizeInBytes;
            Name = name;
            Path = path;
            MimeType = mimeType;
            Accessibility = accessibility;
        }


        #region Infr
        private Media() : base(Guid.Empty) { }
        #endregion
    }
}