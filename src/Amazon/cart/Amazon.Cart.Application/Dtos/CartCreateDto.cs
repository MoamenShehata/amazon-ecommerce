namespace Amazon.Cart.Application.Dtos
{
    public record CartCreateDto(CartItemCreateDto CartItem);
    public record CartCreateResultDto(Guid CartId, CartItemDto CartItem);
}