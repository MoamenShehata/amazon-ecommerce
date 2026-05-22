namespace Amazon.Customers.Domain.ValueObjects;

public class CityInfo
{
    public int CountryId { get; private set; }
    public int CityId { get; private set; }
    public string PostalCode { get; private set; }

    public CityInfo(int countryId, int cityId, string postalCode)
    {
        CountryId = countryId;
        CityId = cityId;
        PostalCode = postalCode;
    }
}

public class HouseInfo
{
    public string Street { get; private set; }
    public int BuildingNumber { get; private set; }
    public int? ApartmentNumber { get; private set; }

    public HouseInfo(string street, int buildingNumber, int? apartmentNumber)
    {
        Street = street;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
    }
}