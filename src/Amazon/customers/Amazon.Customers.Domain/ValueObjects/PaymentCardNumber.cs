namespace Amazon.Customers.Domain.ValueObjects;

public record PaymentCardNumber
{
    private const int ValidLength = 16;
    private const int MaskLength = 12;

    private string _cardNumber;
    public string Value
    {
        get
        {
            return _cardNumber.ToString().Replace(_cardNumber.Substring(0, MaskLength), string.Join("", Enumerable.Range(1, MaskLength).Select(x => "*")));
        }

        private set
        {
            _cardNumber = value;
        }
    }

    public PaymentCardNumber(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(PaymentCardNumber));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, ValidLength, nameof(PaymentCardNumber));
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, ValidLength, nameof(PaymentCardNumber));

        Value = value;
    }
}
