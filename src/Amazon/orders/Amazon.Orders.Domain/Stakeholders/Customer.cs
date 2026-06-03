using Amazon.Orders.Domain.Orders;
using Amazon.SharedKernel.API;

namespace Amazon.Orders.Domain.Stakeholders;

public class Customer : StakeHolder
{
    public Customer(Guid userId) : base(userId)
    {
    }

    public override RestResponse<Order> CanAccessOrder(Order order)
    {
        if (Id != order.Owner.Id)
            return RestResponse<Order>.BadRequest(new BadRequestModel("Order is not owned by requester customer"));

        return RestResponse<Order>.Success(order);
    }

    private Customer() : base(Guid.Empty)
    {

    }
}
