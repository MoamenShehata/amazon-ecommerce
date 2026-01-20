namespace Amazon.Cart.Application.Dtos
{
    public record CartCreateDto(Guid? CustomerId, CartItemCreateDto CartItem);
}