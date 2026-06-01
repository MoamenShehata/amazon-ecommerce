using Amazon.Customers.Domain.Entities;
using Amazon.Customers.Domain.Events;
using Amazon.Customers.Domain.ValueObjects;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Customers.Domain;

public class Customer : AuditableAggregate<Guid>, IEntity<Guid>
{
    public ContactInfo ContactInfo { get; private set; }

    public Customer(Guid id, ContactInfo contactInfo) : base(id)
    {
        ContactInfo = contactInfo;
        ShippingInfo = new();
    }

    public ShippingInfo ShippingInfo { get; private set; }
    public RestResponse<bool> AddShippingAddress(ShippingAddress newAddress)
    {
        var addAddressResult = ShippingInfo.AddAddress(newAddress);
        if (!addAddressResult.IsSuccess)
            return RestResponse<bool>.BadRequest(addAddressResult.Error.ToString());

        RaiseEvent(new CustomerShippingInfoChangedEvent(Id));
        return RestResponse<bool>.Success(true);
    }

    public RestResponse<bool> RemoveShippingAddress(int addressId)
    {
        var removeAddressResult = ShippingInfo.RemoveAddress(addressId);
        if (!removeAddressResult.IsSuccess)
            return RestResponse<bool>.BadRequest(removeAddressResult.Error.ToString());

        RaiseEvent(new CustomerShippingInfoChangedEvent(Id));
        return RestResponse<bool>.Success(true);
    }


    private readonly ICollection<PaymentCard> _paymentCards = [];
    public IReadOnlyCollection<PaymentCard> PaymentCards => _paymentCards.ToList().AsReadOnly();

    public RestResponse<PaymentCard> AddPaymentCard(PaymentCardInfo cardInfo)
    {
        if (_paymentCards.Count == 3)
            return RestResponse<PaymentCard>.BadRequest("A customer cannot have more than 3 payment cards.");

        try
        {
            var newCard = new PaymentCard(Id, cardInfo);
            _paymentCards.Add(newCard);
            return RestResponse<PaymentCard>.Success(newCard);
        }
        catch (Exception ex)
        {
            return RestResponse<PaymentCard>.Failure(ex);
        }
    }

    public RestResponse RemovePaymentCard(int cardId)
    {
        var cardToRemove = _paymentCards.FirstOrDefault(c => c.Id == cardId);
        if (cardToRemove == null)
            return RestResponse.BadRequest(new BadRequestModel("Payment card not found."));

        _paymentCards.Remove(cardToRemove);
        return RestResponse<bool>.Success(true);
    }


    #region Infrastructure

    private Customer() : base(Guid.Empty)
    {

    }
    #endregion
}
