using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Media.Events
{
    public record MediaCreateRequestedEvent(Guid MediaId, Guid OwnerId, Stream Stream, DateTime OccurredOn, bool IsPublic) : DomainEvent(OccurredOn, true);
}