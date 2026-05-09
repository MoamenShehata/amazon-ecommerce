using Amazon.Inventory.Domain.Products.Entities;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;

namespace Amazon.Inventory.Domain.Products.ValueObjects;

public class ProductInventory
{
    private readonly List<InventoryItem> _items = [];

    public ProductInventory(Guid productId, int inStockCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(inStockCount, 0, nameof(inStockCount));

        InsertProductItems(productId, inStockCount);
    }

    private void InsertProductItems(Guid productId, int quantity)
    {
        for (int i = 1; i <= quantity; i++)
            _items.Add(new InventoryItem(productId));
    }

    public ProductInventory(ProductInventory newValue) : this(newValue.ProductId, newValue.InStockCount) { }

    public int InStockCount => _items.Count;
    public Guid ProductId => _items.FirstOrDefault()?.ProductId ?? throw new InvalidOperationException("No product items available");

    public void Insert(int quantity) => InsertProductItems(ProductId, quantity);

    public RestResponse<bool> Consume(int inventoryItemId)
    {
        var inventoryItem = _items.FirstOrDefault(x => x.Id == inventoryItemId && x.ProductId == ProductId);
        if (inventoryItem is null)
            return RestResponse<bool>.NotFound($"Inventory does not have the specified item");

        _items.Remove(inventoryItem);
        return RestResponse<bool>.Success(true);
    }

    public RestResponse<bool> CanConsume(int quantityToConsume)
    {
        if (InStockCount < quantityToConsume)
            return RestResponse<bool>.BadRequest(new BadRequestModel($"InStock amount of ({InStockCount}) cannot satisfy the required quantity of{quantityToConsume}"));

        return RestResponse<bool>.Success(true);
    }

    public RestResponse<int> HoldItemForPurchase()
    {
        var firstAvailableItem = _items.FirstOrDefault(i => !i.IsOnHold);
        if (InStockCount == 0 || firstAvailableItem is null)
            return RestResponse<int>.NotFound($"No available inventory items to hold for purchase");

        firstAvailableItem.HoldForPurchase();
        return RestResponse<int>.Success(firstAvailableItem.Id);
    }

    public void ReleaseAllOnHoldItems()
    {
        foreach (var item in _items.Where(i => i.IsOnHold))
            item.ReleaseHold();
    }

    private ProductInventory() { }
}