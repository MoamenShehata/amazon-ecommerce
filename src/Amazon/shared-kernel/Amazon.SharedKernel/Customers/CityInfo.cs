namespace Amazon.SharedKernel.Customers;

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
