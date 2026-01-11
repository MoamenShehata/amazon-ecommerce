using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Orders.Domain.Orders.Events
{
    public class OrderCreatedEvent : DomainEventBase
    {
        internal OrderCreatedEvent(DateTime occurredOn, Guid OrderId) : base(occurredOn)
        {
        }
    }
}