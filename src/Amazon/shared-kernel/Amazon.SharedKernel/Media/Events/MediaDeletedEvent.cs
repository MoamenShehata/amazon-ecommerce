using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Media.Events
{
    public record MediaDeletedEvent(Guid MediaId, DateTime OccurredOn) : DomainEvent(OccurredOn, true);
}