using Amazon.Cart.Domain.Payments.Factories;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Payments;

public class PaymentsService(
    IRepository<PaymentMethod, Guid> _repository,
   PaymentRequestFactory _paymentRequestFactory)
{
    public async Task<RestResponse<string>> CreatePaymentRequestAsync(Guid paymentMethodId, Guid userId, int? deliverToAddressId)
    {
        var method = await _repository.GetInstanceAsync(paymentMethodId);
        if (method is null)
            return RestResponse<string>.NotFound(new { Message = $"Payment method with id {paymentMethodId} not found" });

        var paymentRequest = await _paymentRequestFactory.CreateAsync(method, userId, deliverToAddressId);

        return RestResponse<string>.Success(method.RedirectToAppUrlPath);
    }
}