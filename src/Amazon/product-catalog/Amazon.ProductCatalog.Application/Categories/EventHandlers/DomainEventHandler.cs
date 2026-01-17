//using MediatR;
//using Moamen.SDKs.SharedKernel;
//using Moamen.SDKs.SharedKernel.DDD.Events;

//namespace Amazon.ProductCatalog.Application.Categories.EventHandlers;

//public class DomainEventHandler<TDomainEvent>(
//    EventStoreService _eventStoreService,
//    IUnitOfWork _unitOfWork
//    )
//    : INotificationHandler<TDomainEvent>
//    where TDomainEvent : DomainEventBase
//{
//    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
//    {
//        _eventStoreService.Append(notification);

//        await _unitOfWork.CommitAsync();
//    }
//}
