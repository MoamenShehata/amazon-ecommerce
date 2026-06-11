using Amazon.Notifications.Api.SignalR.Hubs;
using Amazon.SharedKernel.Orders.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Amazon.Notifications.Api.EventConsumers
{
    public class SendUiNotificationOrderCreatedEventHandler(IHubContext<NotificationsHub> _hubContext) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;

            await _hubContext.Clients.User("mo@mo.com").SendAsync("UserMessage", $"Your order was created at pending status (orderId):{message.OrderId}");
        }
    }
}
