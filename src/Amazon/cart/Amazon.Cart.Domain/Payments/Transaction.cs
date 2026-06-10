using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.Payments;

public class Transaction : AuditableAggregate<Guid>, IEntity<Guid>
{
    public decimal Amount { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public int? PaymentCardId { get; private set; }
    public string? PaymentCardNumberMasked { get; private set; }
    public string? CheckedoutSessionId { get; private set; }

    public Transaction(decimal amount, Guid orderId, Guid customerId, int paymentCardId, string paymentCardNumberMasked) : base(Guid.NewGuid())
    {
        Amount = amount;
        OrderId = orderId;
        CreatedOn = DateTime.UtcNow;
        CreatedBy = customerId.ToString();
        CustomerId = customerId;
        PaymentCardId = paymentCardId;
        PaymentCardNumberMasked = paymentCardNumberMasked;
    }

    public Transaction(decimal amount, Guid orderId, Guid customerId, string checkedoutSessionId) : base(Guid.NewGuid())
    {
        Amount = amount;
        OrderId = orderId;
        CreatedOn = DateTime.UtcNow;
        CreatedBy = customerId.ToString();
        CustomerId = customerId;
        CheckedoutSessionId = checkedoutSessionId;
    }
}