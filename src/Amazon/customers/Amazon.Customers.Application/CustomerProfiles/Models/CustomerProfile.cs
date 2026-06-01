using Amazon.Customers.Application.Dtos;

namespace Amazon.Customers.Application.CustomerProfiles.Models;

public class CustomerProfile
{
    public Guid CustomerId { get; set; }
    public ICollection<CustomerProfileAddress> Addresses { get; set; } = new HashSet<CustomerProfileAddress>();
    public ICollection<PaymentCardDto> PaymentCards { get; set; } = [];
}

public class CustomerProfileAddress
{
    public int Id { get; set; }
    public string Country { get; private set; }
    public string City { get; private set; }
    public string PhoneNumber { get; private set; }
    public string PostalCode { get; private set; }
    public string Street { get; private set; }
    public int BuildingNumber { get; private set; }
    public int? ApartmentNumber { get; private set; }

    public CustomerProfileAddress(int id, string country, string city, string street, int buildingNumber, int? apartmentNumber, string phoneNumber, string postalCode)
    {
        Id = id;
        Country = country;
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        PhoneNumber = phoneNumber;
        PostalCode = postalCode;
    }

    public string Value => $"{Country} - {City} - {Street} - {BuildingNumber} - {ApartmentNumber}";
}

