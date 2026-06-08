using Amazon.Inventory.Domain.Products.Entities;
using Amazon.SharedKernel.API;

namespace Amazon.Inventory.Domain.Products.ValueObjects;

public class ProductInventory
{
    private readonly List<InventoryItem> _items = [];
    private IEnumerable<InventoryItem> AvailableItems => _items.Where(x => x.IsAvailable);
    public IReadOnlyCollection<InventoryItem> Items => _items.ToList();
    internal IReadOnlyCollection<InventoryItem> OrderItems(Guid orderId) => [.. _items.Where(x => x.ReservedForOrder == orderId)];


    public int InStockCount => AvailableItems.Count();

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

    internal RestResponse<bool> ReserveQuantityForOrder(Guid orderId, int quantity)
    {
        var isStockCountEnoughResult = CanConsume(quantity);
        if (!isStockCountEnoughResult.IsSuccess)
            return isStockCountEnoughResult;

        foreach (var item in AvailableItems.Take(quantity))
            item.ReserveForOrder(orderId);

        return RestResponse<bool>.Success(true);
    }

    public ProductInventory(ProductInventory newValue) : this(newValue.ProductId, newValue.InStockCount) { }

    public Guid ProductId => _items.FirstOrDefault()?.ProductId ?? throw new InvalidOperationException("No product items available");

    public void Insert(int quantity) => InsertProductItems(ProductId, quantity);

    public RestResponse<bool> ConsumeForOrder(Guid orderId)
    {
        var orderReserverdItems = OrderItems(orderId);
        foreach (var item in orderReserverdItems)
            _items.Remove(item);

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
        var firstAvailableItem = _items.FirstOrDefault(i => i.IsAvailable);
        if (InStockCount == 0 || firstAvailableItem is null)
            return RestResponse<int>.NotFound($"No available inventory items to hold for purchase");

        firstAvailableItem.ReserveForOrder(Guid.Empty);
        return RestResponse<int>.Success(firstAvailableItem.Id);
    }

    internal void ReleaseReservedItems()
    {
        foreach (var item in _items.Where(i => !i.IsAvailable))
            item.Release();
    }

    public bool HasReservedItems => _items.Any(i => i.ReservedForOrder.HasValue);

    private ProductInventory() { }
}