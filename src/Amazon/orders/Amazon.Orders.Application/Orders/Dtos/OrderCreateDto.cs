using Amazon.SharedKernel.Customers;

namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderCreateDto(Guid OrderId, List<KeyValuePair<Guid, int>> ShoppingCart, DeliveryAddress DeliveryAddress);
}