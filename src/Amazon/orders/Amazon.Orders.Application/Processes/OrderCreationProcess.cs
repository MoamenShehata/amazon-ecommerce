using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Amazon.SharedKernel.Orders.Commands;
using Amazon.SharedKernel.Orders.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Orders.Application.Processes;

// order creation business process orchestator SAGA
public class OrderCreationProcess(
    IRepository<Order, Guid> _orders,
    IUnitOfWork _unitOfWork,
    EventStoreService _eventStoreService
    ) : IConsumer<OrderPaymentConfirmedEvent>,
    IConsumer<InventoryReservedEvent>, IConsumer<InventoryReservationFailedEvent>,
    IConsumer<ShipmentCreatedEvent>, IConsumer<CreateShipmentFailedEvent>
{
    public async Task Consume(ConsumeContext<OrderPaymentConfirmedEvent> context)
    {
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);
        order.ConfirmPayment(context.Message.OccurredOn, context.Message.PaymentInfo);

        _eventStoreService.Append(new ReserveInventoryCommand(order.Id, order.Items.Select(x => new KeyValuePair<Guid, int>(x.ProductInfo.ProductId, x.Quantity)).ToList()));

        await _unitOfWork.CommitAsync();
    }

    public async Task Consume(ConsumeContext<InventoryReservedEvent> context)
    {
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);
        order.TryUpdateTo(OrderState.Processing, null);

        _eventStoreService.Append(new CreateShipmentCommand(order.Id, order.Owner, order.DeliveryAddress));

        await _unitOfWork.CommitAsync();
    }

    public async Task Consume(ConsumeContext<InventoryReservationFailedEvent> context)
    {
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);
        order.RequestCompensation();
        await _unitOfWork.CommitAsync();
    }

    public async Task Consume(ConsumeContext<ShipmentCreatedEvent> context)
    {
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);
        //update order status

        await _unitOfWork.CommitAsync();
    }

    public async Task Consume(ConsumeContext<CreateShipmentFailedEvent> context)
    {
        // we should retry TBH, as what could happen in just creating a shipment request for me to compensate and cancel the order???
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);

        await _unitOfWork.CommitAsync();
    }
}