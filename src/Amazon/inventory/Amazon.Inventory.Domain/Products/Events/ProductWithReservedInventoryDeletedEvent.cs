using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Inventory.Domain.Products.Events;

internal record ProductWithReservedInventoryDeletedEvent(Guid ProductId) : IntegrationEvent(DateTime.UtcNow);
