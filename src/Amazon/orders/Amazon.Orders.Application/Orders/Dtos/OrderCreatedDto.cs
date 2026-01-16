namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderCreatedDto(Guid Id);
    public record OrderDto(Guid Id, List<KeyValuePair<string, int>> Items);
}