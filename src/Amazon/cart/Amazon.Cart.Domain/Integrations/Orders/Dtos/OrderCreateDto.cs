using Amazon.SharedKernel.Customers;

namespace Amazon.Cart.Domain.Integrations.Orders.Dtos;

public record OrderCreateDto(
    Guid OrderId,
    List<KeyValuePair<Guid, int>> ShoppingCart,
    DeliveryAddress DeliveryAddress);
