using Amazon.Customers.Domain.ValueObjects;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Customers.Domain.Entities;

public class PaymentCard : AuditableEntity<int>
{
    public Guid CustomerId { get; private set; }

    public PaymentCardInfo Info { get; private set; }
    public PaymentCardState State { get; private set; }

    public PaymentCard(Guid customerId, PaymentCardInfo info) : base(0)
    {
        Info = info;
        State = PaymentCardState.OfActive;
    }

    private PaymentCard() : base(0)
    {

    }
}