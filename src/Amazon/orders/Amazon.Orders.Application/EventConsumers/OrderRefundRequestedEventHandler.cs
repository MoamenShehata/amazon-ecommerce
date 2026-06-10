using Amazon.Orders.Domain.Orders;
using Amazon.SharedKernel.Orders.Events;
using MassTransit;
using Moamen.SDKs.Repository;

namespace Amazon.Orders.Application.EventConsumers;

internal class OrderRefundRequestedEventHandler(
    IRepository<Order, Guid> _orders
    ) : IConsumer<OrderRefundRequestedEvent>
{
    public async Task Consume(ConsumeContext<OrderRefundRequestedEvent> context)
    {
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);

        var paymentInfo = order.Info;

        if (paymentInfo is PaymentCardCheckoutInfo cardCheckoutInfo)
        {
            // use cardCheckoutInfo to debit the customer`s card with the amount
        }
        else if (paymentInfo is PaymentGatewayCheckoutInfo gatewayCheckoutInfo)
        {
            // call the used gateway to ask for refund
        }
    }
}