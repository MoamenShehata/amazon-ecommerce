namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderCreateDto(Guid UserId, string Email, List<KeyValuePair<Guid, int>> ShoppingCart, object PaymentInfo, object DeliveryAddress);
}