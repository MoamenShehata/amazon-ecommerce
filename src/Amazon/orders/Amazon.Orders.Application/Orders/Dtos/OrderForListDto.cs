namespace Amazon.Orders.Application.Orders.Dtos
{
    public record OrderForListDto(Guid Id, DateTime CreatedAt, string Status, string CreatedByEmail);
}