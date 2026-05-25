using Amazon.Orders.Application.Orders.Dtos;
using Amazon.Orders.Domain.Orders;

namespace Amazon.Orders.Application.Orders.Mappers;

public static class OrderMappers
{
    public static OrderForListDto ToForListDto(this Order o)
    {
        return new OrderForListDto(o.Id, o.CreatedOn, o.Status.ToString());
    }

    public static OrderDetailsDto ToDetailsDto(this Order o)
    {
        return new OrderDetailsDto(o.Id, o.CreatedOn, o.Status.ToString(), o.Status.AdditionalInfo, o.Price, o.Status.CanBeCancelled, o.Items.Select(i => new OrderItemDto(i.ProductInfo.Name, "", i.ProductInfo.UnitPrice, i.Quantity)).ToList());
    }
}