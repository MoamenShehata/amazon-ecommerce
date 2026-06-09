using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.Payments;

public class PaymentMethod : AuditableAggregate<Guid>, IEntity<Guid>
{
    public string Name { get; private set; }
    public PaymentMehodType Type { get; private set; }

    internal PaymentMethod(string name, PaymentMehodType type) : base(Guid.NewGuid())
    {
        Name = name;
        Type = type;
    }

    public static PaymentMethod ForCash() => new("Cash On Delivery", PaymentMehodType.Cash);
    public static PaymentMethod ForVisa() => new("Payment Card", PaymentMehodType.Visa);

    #region Infra
    private PaymentMethod() : base(Guid.Empty) { }

    #endregion
}