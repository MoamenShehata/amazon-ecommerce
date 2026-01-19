using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Media.Events
{
    public record MediaCreateRequestedEvent(Guid MediaId, Guid OwnerId, byte[] Content, bool IsPublic) : IntegrationEvent(DateTime.UtcNow);
}