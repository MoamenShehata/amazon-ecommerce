using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Inventory.Domain.Products.Events
{
    public class ProductInventoryUpdatedEvent : DomainEventBase
    {
        public ProductInventoryUpdatedEvent(Guid productId, int currentInventory) : base(DateTime.UtcNow, true)
        {
            ProductId = productId;
            CurrentInventory = currentInventory;
        }

        public Guid ProductId { get; }
        public int CurrentInventory { get; }
    }
}