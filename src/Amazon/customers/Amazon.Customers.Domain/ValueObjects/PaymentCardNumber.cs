namespace Amazon.Customers.Domain.ValueObjects;

public class PaymentCardNumber
{
    private const int ValidLength = 16;

    public string Value { get; private set; }

    public PaymentCardNumber(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(PaymentCardNumber));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, ValidLength, nameof(PaymentCardNumber));
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, ValidLength, nameof(PaymentCardNumber));

        Value = value;
    }
}
