using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Media.Events
{
    public record MediaCreatedEvent(Guid MediaId, Guid OwnerId, string FilePath) : IntegrationEvent(DateTime.UtcNow);
}