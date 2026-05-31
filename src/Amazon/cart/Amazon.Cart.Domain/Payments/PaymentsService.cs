using Amazon.Cart.Domain.Payments.Entities;
using Amazon.Cart.Domain.Payments.Factories;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Cart.Domain.Payments;

public class PaymentsService(
    IRepository<PaymentMethod, Guid> _repository,
    IRepository<PayemntRequest, Guid> _payemntRequestsRepository,
   PaymentRequestFactory _paymentRequestFactory)
{
    public async Task<RestResponse<string>> CreatePaymentRequestAsync(Guid paymentMethodId, Guid userId, int? deliverToAddressId)
    {
        var method = await _repository.GetInstanceAsync(paymentMethodId);
        if (method is null)
            return RestResponse<string>.NotFound(new { Message = $"Payment method with id {paymentMethodId} not found" });

        var paymentRequest = await _paymentRequestFactory.CreateAsync(method, userId, deliverToAddressId);

        return RestResponse<string>.Success($"{method.RedirectToAppUrlPath}/{paymentRequest.Id}");
    }

    public async Task ConfirmPaymentAsync(Guid paymentRequestId)
    {
        var paymentRequest = await _payemntRequestsRepository.GetInstanceAsync(paymentRequestId);
        if (paymentRequest is null) return;

        paymentRequest.Confirm();
    }
}