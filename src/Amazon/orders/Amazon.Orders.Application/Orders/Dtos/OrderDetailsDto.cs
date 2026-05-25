namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderDetailsDto(Guid Id, List<OrderItemDto> Items);
}