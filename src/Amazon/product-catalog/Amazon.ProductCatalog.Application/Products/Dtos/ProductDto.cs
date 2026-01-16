using Amazon.ProductCatalog.Domain.Products.ValueObjects;

namespace Amazon.ProductCatalog.Application.Products.Dtos
{
    public record ProductDto(
        Guid Id,
        Guid CategoryId,
        string Category,
        string Name,
        decimal Price,
        List<ProductProperty> Properties,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public record ProductPropertyDto(string Name, string Value);

    public record CreateProductDto(
        Guid CategoryId,
        string Name,
        int InStockCount,
        decimal Price,
        decimal MinimumPrice,
        decimal MaximumPrice,
        List<ProductProperty> Properties
    );

    public record UpdateProductDto(
        string Name,
        decimal Price,
        List<ProductProperty> Properties
    );
}
