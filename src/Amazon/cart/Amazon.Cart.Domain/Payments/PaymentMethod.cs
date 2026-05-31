using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.Payments;

public class PaymentMethod : AuditableAggregate<Guid>, IEntity<Guid>
{
    public string Name { get; private set; }
    public string RedirectToAppUrlPath { get; private set; }
    internal PaymentMehodType Type { get; private set; }

    internal PaymentMethod(string name, PaymentMehodType type, string redirectToAppUrlPath) : base(Guid.NewGuid())
    {
        Name = name;
        Type = type;
        RedirectToAppUrlPath = redirectToAppUrlPath;
    }

    public static PaymentMethod ForCash() => new("Cash On Delivery", PaymentMehodType.Cash, "checkout/cash");
    public static PaymentMethod ForVisa() => new("Payment Card", PaymentMehodType.Visa, "checkout/card");

    #region Infra
    private PaymentMethod() : base(Guid.Empty) { }

    #endregion
}