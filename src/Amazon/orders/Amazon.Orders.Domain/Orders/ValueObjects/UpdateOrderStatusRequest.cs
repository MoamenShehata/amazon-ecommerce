using Amazon.Orders.Domain.Orders.ValueObjects.Status;

namespace Amazon.Orders.Domain.Orders.ValueObjects;

public class UpdateOrderStatusRequest
{
    public OrderState To { get; set; }
    public object Payload { get; set; }
}
