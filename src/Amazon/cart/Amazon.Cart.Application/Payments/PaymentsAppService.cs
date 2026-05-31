using Amazon.Cart.Application.Payments.Dtos;
using Amazon.Cart.Application.Services;
using Amazon.Cart.Domain.Payments;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Cart.Application.Payments
{
    public class PaymentsAppService(
        IRepository<PaymentMethod, Guid> _paymentMethodsRepository,
        PaymentsService _paymentsService,
        IUnitOfWork _unitOfWork,
        IAuthenticationService _authenticationService)
    {
        public async Task<RestResponse<List<PaymentMethodDto>>> GetAllAsync()
        {
            var methods = await _paymentMethodsRepository.GetAllAsync();
            return RestResponse<List<PaymentMethodDto>>.Success(methods.Select(m => new PaymentMethodDto(m.Id, m.Name)).ToList());
        }

        public async Task<RestResponse<Guid>> CreatePaymentRequestAsync(Guid paymentMethodId, int? deliverToAddressId)
        {
            var result = await _paymentsService.CreatePaymentRequestAsync(paymentMethodId, _authenticationService.CurrentUser.Id, deliverToAddressId);
            if (!result.IsSuccess)
                return result.MapTo(Guid.Empty);

            await _unitOfWork.CommitAsync();
            return RestResponse<Guid>.Success(result.Value);
        }
    }
}