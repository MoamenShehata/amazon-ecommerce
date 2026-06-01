namespace Amazon.Customers.Domain.ValueObjects;

public class PaymentCardState
{
    private static PaymentCardState _inActive = new PaymentCardState(false);
    private static PaymentCardState _active = new PaymentCardState(true);

    public bool IsActive { get; private set; }

    private PaymentCardState(bool isActive)
    {
        IsActive = isActive;
    }

    public static PaymentCardState OfDefault => _inActive;
    public static PaymentCardState OfActive => _active;
}
