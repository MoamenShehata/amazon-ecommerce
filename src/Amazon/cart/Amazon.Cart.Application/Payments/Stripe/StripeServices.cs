using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Orders.Events;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;
using Stripe;

namespace Amazon.Cart.Application.Payments.Stripe;

public class StripeServices(
        IRepository<ShoppingCart, Guid> _carts,
        EventStoreService _eventStoreService,
        IUnitOfWork _unitOfWork
    )
{
    public async Task<RestResponse<bool>> ProcessStripeCallbackAsync(Event callbackEvent)
    {
        switch (callbackEvent.ExtractPaymentStatus())
        {
            case PaymentStatus.Paid:
                var stripeSessionId = callbackEvent.ExtractCheckoutSessionId();

                var cartBySessionId = await _carts.GetInstanceAsync(x => x.CheckedoutSessionId == stripeSessionId);
                if (cartBySessionId != null)
                {
                    _eventStoreService.Append(new OrderPaymentConfirmedEvent(cartBySessionId.OrderId.Value, new PaymentGatewayCheckoutInfo(stripeSessionId)));

                    _carts.Remove(cartBySessionId);
                    await _unitOfWork.CommitAsync();
                }
                return RestResponse<bool>.Success(true);
                // Find Order by StripeSessionId

                // Mark Order Paid

                // Publish OrderPaid event
                break;
            case PaymentStatus.Failed_Insufficient:

                break;
            case PaymentStatus.Unknown:
                break;
        }

        return RestResponse<bool>.Success(true);
    }
}