namespace Amazon.Customers.Domain.ValueObjects;

public class ContactInfo
{
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    internal ContactInfo(string email, string phoneNumber)
    {
        Email = email;
        PhoneNumber = phoneNumber;
    }
}