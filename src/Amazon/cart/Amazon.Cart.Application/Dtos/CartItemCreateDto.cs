namespace Amazon.Cart.Application.Dtos
{
    public record CartItemCreateDto(Guid ProductId, int Quantity, string ProductName, string ProductImageUrl);
}