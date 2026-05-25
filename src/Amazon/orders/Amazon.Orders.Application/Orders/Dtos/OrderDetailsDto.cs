namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderDetailsDto(Guid Id, DateTime CreatedAt, string Status, object StatusAdditionalInfo, decimal TotalAmount, List<OrderItemDto> Items);
}