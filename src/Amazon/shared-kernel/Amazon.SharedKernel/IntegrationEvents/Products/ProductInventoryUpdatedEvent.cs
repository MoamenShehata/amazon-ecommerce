namespace Amazon.SharedKernel.IntegrationEvents.Products;

public record ProductInventoryUpdatedIntegrationEvent(Guid ProductId, int CurrentInventory);