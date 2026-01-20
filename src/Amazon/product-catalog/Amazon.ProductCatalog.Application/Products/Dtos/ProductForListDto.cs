namespace Amazon.ProductCatalog.Application.Products.Dtos
{
    public record ProductForListDto(string Name, string Categories, decimal UnitPrice, string? ImageUrl);
}