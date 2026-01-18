using Amazon.ProductCatalog.Domain.Products.ValueObjects;

namespace Amazon.ProductCatalog.Application.Products.Dtos
{
    public record CreateProductDto(
        Guid CategoryId,
        string Name,
        int InStockCount,
        decimal Price,
        decimal MinimumPrice,
        decimal MaximumPrice,
        List<ProductProperty> Properties
    );
}
