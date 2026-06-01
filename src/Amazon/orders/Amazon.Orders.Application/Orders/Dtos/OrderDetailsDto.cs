namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderDetailsDto(
        Guid Id,
        DateTime CreatedAt,
        string Status,
        object StatusAdditionalInfo,
        decimal TotalAmount,
        bool CanBeCanceled,
        List<OrderItemDto> Items,
        string PaymentInfo,
        string DeliveryAddress);
}