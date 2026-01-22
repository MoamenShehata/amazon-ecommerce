namespace Amazon.Cart.Application.Dtos
{
    public record CartProductDto(
        Guid ProductId,
        string ProductName,
        string ProductImageUrl,
        List<int> ItemIds);
}