namespace Amazon.Customers.Application.CustomerProfiles.Models;

public class CustomerProfile
{
    public Guid CustomerId { get; set; }
    public ICollection<CustomerProfileAddress> Addresses { get; set; } = new HashSet<CustomerProfileAddress>();
}

public class CustomerProfileAddress
{
    public string Country { get; private set; }
    public string City { get; private set; }
    public string Street { get; private set; }
    public int BuildingNumber { get; private set; }
    public int? ApartmentNumber { get; private set; }

    public string Value => $"{Country} - {City} - {Street} - {BuildingNumber} - {ApartmentNumber}";
}

