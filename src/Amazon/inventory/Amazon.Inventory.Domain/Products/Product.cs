using Amazon.Inventory.Domain.Products.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Products.Events;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Inventory.Domain.Products;

public class Product : AuditableAggregate<Guid>, IEntity<Guid>
{
    public ProductInventory Inventory { get; private set; }

    private readonly List<ProductInventoryChange> _inventoryChanges = [];

    public Product(Guid id, int inStockCount) : base(id)
    {
        Inventory = new(id, inStockCount);
    }

    public void AddToInventory(int quantity)
    {
        Inventory.Insert(quantity);
        UpdateInventory(Inventory.InStockCount, Inventory.InStockCount + quantity);
    }

    public RestResponse<bool> ReserveQuantityForOrder(Guid orderId, int quantity) => Inventory.ReserveQuantityForOrder(orderId, quantity);
    public void ReleaseReservedItems() => Inventory.ReleaseReservedItems();

    public RestResponse<bool> ConsumeForOrder(Guid orderId)
    {
        var inventoryItemsBeforeConsume = Inventory.OrderItems(orderId).Count;

        var consumeResult = Inventory.ConsumeForOrder(orderId);
        if (!consumeResult.IsSuccess)
            return RestResponse<bool>.BadRequest(new BadRequestModel(consumeResult.Error.ToString()!));

        UpdateInventory(inventoryItemsBeforeConsume, Inventory.InStockCount);
        return RestResponse<bool>.Success(true);
    }

    private void UpdateInventory(int beforeQuantity, int newQuantity)
    {
        _inventoryChanges.Add(new(Id, beforeQuantity, newQuantity));

        RaiseEvent(new ProductInventoryUpdatedEvent(Id, newQuantity));
    }

    #region Infra
    private Product() : base(Guid.Empty) { }
    #endregion
}
