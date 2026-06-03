using Amazon.Orders.Domain.Orders;
using Amazon.SharedKernel.API;

namespace Amazon.Orders.Domain.Stakeholders;

public class DeliveryUser : StakeHolder
{
    public DeliveryUser(Guid userId) : base(userId)
    {
    }

    public override RestResponse<Order> CanAccessOrder(Order order)
    {
        return RestResponse<Order>.Success(order);
    }

    private DeliveryUser() : base(Guid.Empty)
    {

    }
}