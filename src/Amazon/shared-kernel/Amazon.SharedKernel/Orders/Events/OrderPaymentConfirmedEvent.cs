using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Orders.Events;

public record OrderPaymentConfirmedEvent(Guid OrderId, Guid? TransactionId = null) : IntegrationEvent(DateTime.UtcNow);