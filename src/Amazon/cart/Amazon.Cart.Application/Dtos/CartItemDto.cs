namespace Amazon.Cart.Application.Dtos
{
    public record CartItemDto(
        Guid ProductId,
        string ProductName,
        string ProductImageUrl,
        List<int> ItemIds);
}