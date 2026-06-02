namespace Amazon.Cart.Domain.Integrations.Customers.Dtos;

public record CustomerDeliveryAddress(int Id, string PhoneNumber, string Country, string City, string PostalCode, string Street, int BuildingNumber)
{
    public object AsOrderDeliveryAddress =>
    new
    {
        PhoneNumber,
        Country,
        City,
        PostalCode,
        Street,
        BuildingNumber
    };
}