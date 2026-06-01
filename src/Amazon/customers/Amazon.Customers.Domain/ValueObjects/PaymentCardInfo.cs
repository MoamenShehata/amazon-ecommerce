namespace Amazon.Customers.Domain.ValueObjects;

public class PaymentCardInfo
{
    public string HolderName { get; private set; }
    public PaymentCardNumber Number { get; private set; }
    public PaymentCardExpiration Expiration { get; private set; }

    public PaymentCardInfo(string holderName, PaymentCardNumber number, PaymentCardExpiration expiration)
    {
        ArgumentException.ThrowIfNullOrEmpty(holderName, nameof(holderName));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(holderName.Length, 20);

        HolderName = holderName;
        Number = number;
        Expiration = expiration;
    }
    private PaymentCardInfo()
    {
        
    }
}
