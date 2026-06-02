namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderCreateDto(List<KeyValuePair<Guid, int>> ShoppingCart, object PaymentInfo, object DeliveryAddress);
}