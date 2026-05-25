namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderItemDto(string ProductName, string ProductImage, decimal UnitPrice, int Quantity);
}