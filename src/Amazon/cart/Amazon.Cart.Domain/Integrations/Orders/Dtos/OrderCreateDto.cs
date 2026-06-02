namespace Amazon.Cart.Domain.Integrations.Orders.Dtos;

public record OrderCreateDto(
    List<KeyValuePair<Guid, int>> ShoppingCart,
    object PaymentInfo,
    object DeliveryAddress);
