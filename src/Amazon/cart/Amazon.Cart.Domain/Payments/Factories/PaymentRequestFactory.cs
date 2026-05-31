using Amazon.Cart.Domain.Integrations;
using Amazon.Cart.Domain.Payments.Entities;
using Amazon.Cart.Domain.Payments.ValueObjects;
using Amazon.SharedKernel.Common.Services;
using Moamen.SDKs.Repository;
using System.Text.Json;

namespace Amazon.Cart.Domain.Payments.Factories;

public class PaymentRequestFactory(
    PaymentRequestPayloadFactory _paymentRequestPayloadFactory,
    ICustomerService _customerService,
    IRepository<PayemntRequest, Guid> _repository,
    IOtpService _otpService,
    ISmsService _smsService
    )
{
    public async Task<PayemntRequest> CreateAsync(PaymentMethod paymentMethod, Guid userId, int? deliverToAddressId)
    {
        var customerDeliveryAddress = await _customerService.GetCustomerDeliveryAddressOrDefaultAsync(userId, deliverToAddressId);

        switch (paymentMethod.Type)
        {
            case PaymentMehodType.Cash:
                var paymentRequest = new PayemntRequest(userId, paymentMethod.Id, _paymentRequestPayloadFactory.OfCash(customerDeliveryAddress.Value.PhoneNumber, customerDeliveryAddress.Value.Country, customerDeliveryAddress.Value.City, customerDeliveryAddress.Value.PostalCode, customerDeliveryAddress.Value.Street, customerDeliveryAddress.Value.BuildingNumber));
                _repository.Add(paymentRequest);
                var otp = await _otpService.GenerateAsync(userId);
                await _smsService.SendMessageAsync(customerDeliveryAddress.Value.PhoneNumber, $"Your OTP for confirming your cash payment is: {otp}");
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