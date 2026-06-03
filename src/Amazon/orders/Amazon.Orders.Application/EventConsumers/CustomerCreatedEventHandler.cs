using Amazon.Orders.Domain.Stakeholders;
using Amazon.SharedKernel.Customers.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Application.EventConsumers;

public class OrdersNewCustomerRegistrationEventHandler(
    IRepository<Customer, Guid> _customers,
    IUnitOfWork _unitOfWork
    ) : IConsumer<NewCustomerRegistrationEvent>
{
    public async Task Consume(ConsumeContext<NewCustomerRegistrationEvent> context)
    {
        _customers.Add(new Customer(context.Message.Id));
        await _unitOfWork.CommitAsync();
    }
}
