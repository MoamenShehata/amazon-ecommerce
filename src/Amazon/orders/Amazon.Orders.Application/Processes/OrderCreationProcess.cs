using Amazon.Orders.Domain.Orders;
using Amazon.SharedKernel.Orders.Commands;
using Amazon.SharedKernel.Orders.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Application.Processes;

// order creation business process orchestator SAGA
public class OrderCreationProcess(
    IRepository<Order, Guid> _orders,
    IUnitOfWork _unitOfWork
    ) : IConsumer<OrderCreatedEvent>,
    IConsumer<InventoryReservedEvent>, IConsumer<InventoryReservationFailedEvent>,
    IConsumer<ShipmentCreatedEvent>, IConsumer<CreateShipmentFailedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);
        order.RaiseEvent(new ReserveInventoryCommand(context.Message.OrderId, context.Message.OrderItems));

        await _unitOfWork.CommitAsync();
    }

    public async Task Consume(ConsumeContext<InventoryReservedEvent> context)
    {
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);
        order.RaiseEvent(new CreateShipmentCommand(context.Message.OrderId));

        await _unitOfWork.CommitAsync();
    }

    public async Task Consume(ConsumeContext<InventoryReservationFailedEvent> context)
    {
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);
        // depends how wer gonna handle it
            // maybe serve the customer his products somehwre else
            // or maybe update the ui to mark what items are not available and tell the customer they can contnue but with lack of some products
            // or maybe cancel the request as a whole
        //order.RaiseEvent(new ReserveInventoryCommand(context.Message.OrderId, context.Message.OrderItems));

        await _unitOfWork.CommitAsync();
    }

    public async Task Consume(ConsumeContext<ShipmentCreatedEvent> context)
    {
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);
        order.RaiseEvent(new CreateShipmentCommand(context.Message.OrderId));

        await _unitOfWork.CommitAsync();
    }

    public async Task Consume(ConsumeContext<CreateShipmentFailedEvent> context)
    {
        // we should retry TBH, as what could happen in just creating a shipment request for me to compensate and cancel the order???
        var order = await _orders.GetInstanceAsync(context.Message.OrderId);
        order.RaiseEvent(new CreateShipmentCommand(context.Message.OrderId));

        await _unitOfWork.CommitAsync();
    }


}