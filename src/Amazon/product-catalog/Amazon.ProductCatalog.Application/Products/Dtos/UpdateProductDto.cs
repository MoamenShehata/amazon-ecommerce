using Amazon.ProductCatalog.Domain.Products.ValueObjects;

namespace Amazon.ProductCatalog.Application.Products.Dtos
{
    public record UpdateProductDto(
        string Name,
        decimal Price,
        List<ProductProperty> Properties
    );
}
