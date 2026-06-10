namespace Amazon.Orders.Domain.Orders.ValueObjects.Status;

public enum OrderState
{
    Pending = 0,
    Processing = 1,
    ShippingStarted = 2,
    Shipped = 3,
    DeliveryRecieved = 4,
    CustomerDelivered = 5,
    Cancelled = 6,
    Abandoned = 7,
}
