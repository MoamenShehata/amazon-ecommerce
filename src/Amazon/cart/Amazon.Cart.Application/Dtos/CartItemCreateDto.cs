namespace Amazon.Cart.Application.Dtos
{
    public record CartItemCreateDto(Guid ProductId, int Quantity);
}