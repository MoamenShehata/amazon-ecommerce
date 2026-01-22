namespace Amazon.Cart.Application.Dtos
{
    public record CartItemCreateDto(Guid ProductId, string ProductName, string ProductImageUrl);
}