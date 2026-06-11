namespace Amazon.Cart.Application.Dtos
{
    public record CartItemDto(
        Guid ProductId,
        string ProductName,
        string ProductImageUrl,
        int Quantity,
        decimal UnitPrice,
        bool IsAvailable);
}