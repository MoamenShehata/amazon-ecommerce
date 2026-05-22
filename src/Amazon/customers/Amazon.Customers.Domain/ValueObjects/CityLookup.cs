namespace Amazon.Customers.Domain.ValueObjects;

public class CityLookup
{
    public string CountryName { get; private set; }
    public string CityName { get; private set; }
    public int CityId { get; private set; }

    public CityLookup(string countryName, string cityName, int cityId)
    {
        CountryName = countryName;
        CityName = cityName;
        CityId = cityId;
    }
}
