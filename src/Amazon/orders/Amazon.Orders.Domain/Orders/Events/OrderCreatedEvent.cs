using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Orders.Domain.Orders.Events
{
    public class OrderCreatedEvent : DomainEventBase
    {
        public Guid OrderId { get; }
        public List<KeyValuePair<Guid, int>> OrderItems { get; }

        internal OrderCreatedEvent(DateTime occurredOn, Guid orderId, List<KeyValuePair<Guid, int>> orderItems) : base(occurredOn, true)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }

    }
}