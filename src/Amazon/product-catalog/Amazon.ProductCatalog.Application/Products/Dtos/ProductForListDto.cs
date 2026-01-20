namespace Amazon.ProductCatalog.Application.Products.Dtos
{
    public record ProductForListDto(Guid Id, string Name, string Categories, decimal UnitPrice, string? ImageUrl);
}