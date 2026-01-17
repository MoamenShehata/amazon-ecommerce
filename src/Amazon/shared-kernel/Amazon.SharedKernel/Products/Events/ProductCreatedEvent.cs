using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Products.Events;

public record ProductCreatedEvent
    (Guid CategoryId, 
    Guid ProductId, 
    string Name, 
    int InStockCount, 
    decimal UnitPrice) : IntegrationEvent(DateTime.UtcNow);