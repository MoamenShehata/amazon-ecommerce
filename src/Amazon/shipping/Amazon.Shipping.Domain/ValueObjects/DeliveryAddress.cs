namespace Amazon.Shipping.Domain.ValueObjects;

public class DeliveryAddress
{
    public DeliveryAddress(AddressCity city, AddressAppartment appartment)
    {
        City = city;
        Appartment = appartment;
    }

    public AddressCity City { get; private set; }
    public AddressAppartment Appartment { get; private set; }

}

public class AddressCity
{
    public AddressCity(int countryId, int cityId, string postalCode)
    {
        CountryId = countryId;
        CityId = cityId;
        PostalCode = postalCode;
    }

    public int CountryId { get; private set; }
    public int CityId { get; private set; }
    public string PostalCode { get; private set; }
}

public class AddressAppartment
{
    public AddressAppartment(int buildingNumber, int? apartmentNumber)
    {
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
    }

    public int BuildingNumber { get; private set; }
    public int? ApartmentNumber { get; private set; }
}
