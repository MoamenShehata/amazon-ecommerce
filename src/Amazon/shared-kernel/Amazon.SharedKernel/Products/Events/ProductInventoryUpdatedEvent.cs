using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Products.Events;

public record ProductInventoryUpdatedEvent(Guid ProductId, int CurrentInventory) : IntegrationEvent(DateTime.UtcNow);