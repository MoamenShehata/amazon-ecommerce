using Amazon.Cart.Domain.Integrations;
using Amazon.Cart.Domain.Payments.Entities;
using Amazon.Cart.Domain.Payments.ValueObjects;
using Moamen.SDKs.Repository;
using System.Text.Json;

namespace Amazon.Cart.Domain.Payments.Factories;

public class PaymentRequestFactory(
    PaymentRequestPayloadFactory _paymentRequestPayloadFactory,
    ICustomerService _customerService,
    IRepository<PayemntRequest, Guid> _repository
    )
{
    public async Task<PayemntRequest> CreateAsync(PaymentMethod paymentMethod, Guid userId, int? deliverToAddressId)
    {
        var customerDeliveryAddress = await _customerService.GetCustomerDeliveryAddressOrDefaultAsync(userId, deliverToAddressId);

        switch (paymentMethod.Type)
        {
            case PaymentMehodType.Cash:
                var payload = new PayWithCashPayload(customerDeliveryAddress.Value.PhoneNumber, customerDeliveryAddress.Value.Country, customerDeliveryAddress.Value.City, customerDeliveryAddress.Value.PostalCode, customerDeliveryAddress.Value.Street, customerDeliveryAddress.Value.BuildingNumber);
                var paymentRequest = new PayemntRequest(userId, paymentMethod.Id, new PaymentRequestPayload(JsonSerializer.Serialize(payload), false));
                _repository.Add(paymentRequest);
                return paymentRequest;
                break;

            case PaymentMehodType.Visa:
                throw new NotImplementedException();
                break;

            default:
                throw new NotImplementedException();
                break;
        }
    }
}