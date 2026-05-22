namespace Amazon.Customers.Domain.ValueObjects;

public class CountryLookup
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public List<CityLookup> Cities { get; private set; }

    public CountryLookup(string name, List<CityLookup> cities)
    {
        Name = name;
        Cities = cities;
    }
}
