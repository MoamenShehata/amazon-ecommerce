using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Media.Events
{
    public record MediaCreateRequestedEvent(
        Guid MediaId,
        Guid OwnerId,
        MediaContent Media,
        bool IsPublic)
        : IntegrationEvent(DateTime.UtcNow);
}