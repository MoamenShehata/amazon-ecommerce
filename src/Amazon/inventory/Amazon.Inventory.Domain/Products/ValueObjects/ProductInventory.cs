using Amazon.SharedKernel.API;

namespace Amazon.Inventory.Domain.Products.ValueObjects;

public class ProductInventory
{
    public int InStockCount { get; private set; }
    public ProductInventory(int inStockCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(inStockCount, 0, nameof(inStockCount));

        InStockCount = inStockCount;
    }
    public ProductInventory(ProductInventory newValue) : this(newValue.InStockCount) { }


    public ProductInventory Add(int quantity) => new(InStockCount + quantity);


    public RestResponse<ProductInventory> Consume(int quantityToConsume)
    {
        if (InStockCount < quantityToConsume)
            return RestResponse<ProductInventory>.BadRequest(new BadRequestModel($"InStock amount of ({InStockCount}) cannot satisfy the required quantity of{quantityToConsume}"));

        return RestResponse<ProductInventory>.Success(new(InStockCount - quantityToConsume));
    }

    private ProductInventory() { }
}