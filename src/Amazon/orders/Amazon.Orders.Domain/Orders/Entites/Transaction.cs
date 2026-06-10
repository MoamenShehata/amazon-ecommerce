using Amazon.SharedKernel.Orders.Events;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Orders.Entites;

public class Transaction : AuditableEntity<int>, IEntity<int>
{
    public decimal Amount { get; private set; }
    public Guid OrderId { get; private set; }
    public CheckoutPaymentInfo PaymentInfo { get; private set; }
    internal TransactionType Type { get; private set; }
    internal bool IsArchived { get; private set; }


    private Transaction(Guid orderId, decimal amount, DateTime at, TransactionType type, CheckoutPaymentInfo paymentInfo) : base(0)
    {
        OrderId = orderId;
        Amount = amount;
        CreatedOn = at;
        Type = type;
        PaymentInfo = paymentInfo;
        IsArchived = false;
    }

    internal Transaction(Guid orderId, decimal amount, DateTime at, CheckoutPaymentInfo paymentInfo) : this(orderId, amount, at, TransactionType.Credit, paymentInfo) { }

    public Transaction CreateCompensation()
    {
        if (Type == TransactionType.Debit || IsArchived)
            throw new InvalidOperationException("Cannot compensante thie transaction");

        IsArchived = true;

        return new Transaction(OrderId, Amount, DateTime.UtcNow, TransactionType.Debit, PaymentInfo);
    }

    #region Infra
    private Transaction() : this(Guid.Empty, 0, default, null)
    {

    }
    #endregion
}