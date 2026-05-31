namespace Amazon.Cart.Domain.Payments.ValueObjects;

internal class PayWithCashPayload
{
    internal PayWithCashPayload(string phoneNumber, string country, string city, string postalCode, string street, int buildingNumber)
    {
        PhoneNumber = phoneNumber;
        Country = country;
        City = city;
        PostalCode = postalCode;
        Street = street;
        BuildingNumber = buildingNumber;
    }

    public string PhoneNumber { get; private set; }
    public string Country { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public string Street { get; private set; }
    public int BuildingNumber { get; private set; }

}