using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Commands;

public record ReserveInventoryCommand(Guid OrderId, List<KeyValuePair<Guid, int>> OrderItems) : IntegrationEvent(DateTime.UtcNow);
public record InventoryReservedEvent(Guid OrderId) : IntegrationEvent(DateTime.UtcNow);
public record InventoryReservationFailedEvent(Guid OrderId, List<Guid> ProductOutOfStockIds) : IntegrationEvent(DateTime.UtcNow);
