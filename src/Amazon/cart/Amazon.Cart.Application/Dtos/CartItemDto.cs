namespace Amazon.Cart.Application.Dtos
{
    public record CartItemDto(Guid CartId, Guid ItemId, Guid ProductId, int Quantity);
}