namespace Amazon.SharedKernel.Customers;

public class HouseInfo
{
    public string Street { get; private set; }
    public string PhoneNumber { get; private set; }
    public int BuildingNumber { get; private set; }
    public int? ApartmentNumber { get; private set; }

    public HouseInfo(string street, int buildingNumber, int? apartmentNumber, string phoneNumber)
    {
        Street = street;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        PhoneNumber = phoneNumber;
    }
}
