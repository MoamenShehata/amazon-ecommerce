using Amazon.Cart.Application.Payments.Dtos;
using Amazon.Cart.Domain.Payments;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Application.Payments;

public class PaymentsAppService(
    IRepository<PaymentMethod, Guid> _paymentMethods
    )
{
    public async Task<RestResponse<List<PaymentMethodDto>>> GetPaymentMethodsAsync()
    {
        // should be cashed
        var methods = await _paymentMethods.GetAllAsync();
        return RestResponse<List<PaymentMethodDto>>.Success(methods.Select(m => new PaymentMethodDto(m.Id, m.Name)).ToList());
    }
}