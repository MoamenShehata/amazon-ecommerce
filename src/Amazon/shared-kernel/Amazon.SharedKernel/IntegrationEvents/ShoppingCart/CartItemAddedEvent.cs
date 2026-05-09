using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.IntegrationEvents.ShoppingCart;

public record CartItemAddedEvent(Guid ProductId,int HoldRequestId) : IntegrationEvent(DateTime.UtcNow);
