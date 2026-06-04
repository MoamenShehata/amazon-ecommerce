namespace Amazon.Cart.Domain.Integrations.Orders.Dtos;

public record OrderCreateDto(
    Guid OrderId,
    List<KeyValuePair<Guid, int>> ShoppingCart,
    object PaymentInfo,
    object DeliveryAddress);
