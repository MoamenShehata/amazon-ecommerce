using Amazon.Orders.Domain.Orders;
using Amazon.SharedKernel.API;

namespace Amazon.Orders.Domain.Stakeholders;

public class AdminUser : StakeHolder
{
    public AdminUser(Guid userId) : base(userId)
    {
    }

    public override RestResponse<Order> CanAccessOrder(Order order) => RestResponse<Order>.Success(order);

    private AdminUser() : base(Guid.Empty)
    {

    }
}
