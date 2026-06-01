namespace Amazon.Customers.Domain.ValueObjects;

public class PaymentCardExpiration
{
    public int Month { get; private set; }
    public int Year { get; private set; }

    public PaymentCardExpiration(DateTime expiresAt)
    {
        Month = expiresAt.Month;
        Year = expiresAt.Year;
    }

    public override string ToString() => $"{Month:D2}/{Year}";

    private PaymentCardExpiration()
    {

    }
}