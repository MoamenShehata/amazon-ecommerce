using EMP.SharedKernel.DDD.Definitions;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Amazon.ProductCatalog.Infrastructure.Interceptors;

public class DomainEventsPublisherInterceptor(IMediator _mediator) : SaveChangesInterceptor
{
    public async override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var aggregates = eventData.Context.ChangeTracker.Entries<IAggregate>()
                .Select(x => x.Entity);

        var domainEvents = aggregates.SelectMany(x => x.GetEvents()).ToList();

        foreach (var root in aggregates)
            root.ClearEvents();

        foreach (var @event in domainEvents)
            await _mediator.Publish(@event);
        
        // or save in an event store
        // and later on a background job can handle those events

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}