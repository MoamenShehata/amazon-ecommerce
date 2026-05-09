namespace Amazon.Customers.Domain.ValueObjects;

public class CityInfo
{
    public int CountryId { get; private set; }
    public int CityId { get; private set; }
    public string PostalCode { get; private set; }
}

public class HouseInfo
{
    public string Street { get; private set; }
    public int BuildingNumber { get; private set; }
    public int? ApartmentNumber { get; private set; }
}