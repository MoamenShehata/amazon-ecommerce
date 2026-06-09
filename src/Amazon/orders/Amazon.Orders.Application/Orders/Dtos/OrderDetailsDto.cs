namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderDetailsDto(
        Guid Id,
        DateTime CreatedAt,
        int StatusId,
        string Status,
        object StatusAdditionalInfo,
        decimal TotalAmount,
        bool CanBeCanceled,
        List<OrderItemDto> Items,
        object PaymentInfo,
        object DeliveryAddress);
}