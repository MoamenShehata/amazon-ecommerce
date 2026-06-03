using Amazon.Orders.Domain.Orders;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Stakeholders;

public class StakeHolder : AuditableAggregate<Guid>, IEntity<Guid>
{
    protected StakeHolder(Guid userId) : base(userId) { }

    public virtual RestResponse<Order> CanAccessOrder(Order order) => RestResponse<Order>.Success(order);
    public virtual RestResponse<bool> CanCancelOrder(Order order) => RestResponse<bool>.BadRequest("Order can only be cancelled by it`s owner!");

    private StakeHolder() : base(Guid.Empty)
    {

    }
}