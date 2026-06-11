using Amazon.Cart.Domain.ShoppingCarts;

namespace Amazon.Cart.Domain.Integrations.Orders.Dtos;

public record OrderCreateDto(
    Guid OrderId,
    ShoppingCart ShoppingCart,
    int? DeliveryAddressId);
