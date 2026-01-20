namespace Amazon.Cart.Application.Dtos
{
    public record CartItemDto(Guid CartId, Guid ItemId, Guid ProductId, int Quantity, string ProductName, string ProductImageUrl);
}