using Amazon.Orders.Domain.Stakeholders;
using Amazon.SharedKernel.Customers.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Customers.Application.EventConsumers;

public class AdminUserAddedEventHandler(
    IRepository<AdminUser, Guid> _admins,
    IUnitOfWork _unitOfWork
    ) : IConsumer<AdminUserAddedEvent>
{
    public async Task Consume(ConsumeContext<AdminUserAddedEvent> context)
    {
        _admins.Add(new AdminUser(context.Message.Id));
        await _unitOfWork.CommitAsync();
    }
}