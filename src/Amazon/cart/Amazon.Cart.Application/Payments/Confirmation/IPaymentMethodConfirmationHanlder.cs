using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Orders.Events;

namespace Amazon.Cart.Application.Payments.Confirmation;

public interface IPaymentMethodConfirmationHanlder
{
    Task<RestResponse<CheckoutPaymentInfo>> ConfirmAsync(ConfirmPaymentRequest request, Guid customerId, decimal amount);
}