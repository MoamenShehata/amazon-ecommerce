using Amazon.Cart.Domain.Payments.Factories;
using Amazon.Cart.Domain.Payments.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.Payments.Entities;

public class PayemntRequest : AuditableAggregate<Guid>, IEntity<Guid>
{
    public Guid PaymentMethodId { get; private set; }
    public Guid CustomerId { get; private set; }

    internal PaymentRequestPayload Payload { get; private set; }

    internal PayemntRequest(Guid customerId, Guid paymentMethodId, PaymentRequestPayload payload) : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        PaymentMethodId = paymentMethodId;
        Payload = payload;
    }

    internal PayemntRequest(Guid customerId, Guid paymentMethodId) : this(customerId, paymentMethodId, PaymentRequestPayloadFactory.Empty) { }

    internal void Confirm()
    {
        Payload = Payload.WithConfirmation();
    }

    internal void Confirm(PaymentRequestPayload payload)
    {
        Payload = payload;
        Confirm();
    }

    public bool IsConfirmed => Payload.IsConfirmed;

}