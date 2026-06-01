using Amazon.Cart.Application.Payments.Dtos;
using Amazon.Cart.Application.Services;
using Amazon.Cart.Domain.Payments;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Cart.Application.Payments
{
    public class PaymentsAppService(
        IRepository<PaymentMethod, Guid> _paymentMethodsRepository,
        IUnitOfWork _unitOfWork,
        IAuthenticationService _authenticationService)
    {
        public async Task<RestResponse<List<PaymentMethodDto>>> GetPaymentMethodsAsync()
        {
            var methods = await _paymentMethodsRepository.GetAllAsync();
            return RestResponse<List<PaymentMethodDto>>.Success(methods.Select(m => new PaymentMethodDto(m.Id, m.Name)).ToList());
        }

    }
}