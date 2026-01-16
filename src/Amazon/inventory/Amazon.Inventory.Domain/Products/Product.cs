using Amazon.Inventory.Domain.Products.ValueObjects;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Inventory.Domain.Products;

public class Product : AuditableAggregate<Guid>, IEntity<Guid>
{
    public ProductInventory Inventory { get; private set; }

    private readonly List<ProductInventoryChange> _inventoryChanges = [];

    public Product(Guid id, int inStockCount) : base(id)
    {
        Inventory = new(inStockCount);
    }

    public void AddToInventory(int quantity) => UpdateInventory(Inventory.Add(quantity));

    public RestResponse<bool> ConsumeForOrder(int quantity)
    {
        var consumeResult = Inventory.Consume(quantity);
        if (!consumeResult.IsSuccess)
            return RestResponse<bool>.BadRequest(new BadRequestModel(consumeResult.Error.ToString()!));

        UpdateInventory(consumeResult);
        return RestResponse<bool>.Success(true);
    }

    private void UpdateInventory(ProductInventory newInventory)
    {
        _inventoryChanges.Add(new(Id, Inventory.InStockCount, newInventory.InStockCount));
        Inventory = new(newInventory);
    }

    #region Infra
    private Product() : base(Guid.Empty) { }
    #endregion
}
