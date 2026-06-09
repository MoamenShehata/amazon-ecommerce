using Amazon.SharedKernel.Customers;

namespace Amazon.Cart.Domain.Integrations.Customers.Dtos;

public record CustomerDeliveryAddress(
    int Id,
    string PhoneNumber,
    int CountryId,
    string Country,
    int CityId,
    string City,
    string PostalCode,
    string Street,
    int BuildingNumber)
{
    public DeliveryAddress AsOrderDeliveryAddress =>
    new DeliveryAddress(new CityInfo(CountryId, CityId, PostalCode), new HouseInfo(Street, BuildingNumber, null, PhoneNumber));
}