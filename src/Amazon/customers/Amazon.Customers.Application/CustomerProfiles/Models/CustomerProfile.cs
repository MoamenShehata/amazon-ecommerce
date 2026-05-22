namespace Amazon.Customers.Application.CustomerProfiles.Models;

public class CustomerProfile
{
    public Guid CustomerId { get; set; }
    public ICollection<CustomerProfileAddress> Addresses { get; set; } = new HashSet<CustomerProfileAddress>();
}

public class CustomerProfileAddress
{
    public int? Id { get; set; }
    public string Country { get; private set; }
    public string City { get; private set; }
    public string Street { get; private set; }
    public int BuildingNumber { get; private set; }
    public int? ApartmentNumber { get; private set; }

    public CustomerProfileAddress(string country, string city, string street, int buildingNumber, int? apartmentNumber)
    {
        Country = country;
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
    }

    public string Value => $"{Country} - {City} - {Street} - {BuildingNumber} - {ApartmentNumber}";
}

