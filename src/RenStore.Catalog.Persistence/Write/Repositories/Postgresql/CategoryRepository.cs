using MediatR;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Application.Common;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class CategoryRepository 
    : RenStore.Catalog.Domain.Interfaces.Repository.ICategoryRepository
{
    private readonly IEventStore _eventStore;
    private readonly IMediator _mediator;
    
    public CategoryRepository(
        IEventStore eventStore,
        IMediator mediator)
    { 
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _mediator = mediator     ?? throw new ArgumentNullException(nameof(mediator));
    }
        
    
    public async Task<Category?> GetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        var events = await _eventStore.LoadAsync(id, cancellationToken);

        if (events.Count == 0) return null;
        
        return Category.Rehydrate(events);
    }

    public async Task SaveAsync(
        Category category,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(category);
        
        var uncommittedEvents = category.GetUncommittedEvents();
        
        await _eventStore.AppendAsync(
            aggregateId: category.Id,
            expectedVersion: category.Version,
            events: uncommittedEvents.ToList(),
            cancellationToken);
        
        foreach (var domainEvent in uncommittedEvents)
        {
            var notification = new DomainEventNotification<IDomainEvent>(domainEvent);
            await _mediator.Publish(notification, cancellationToken);
        }
        
        category.UncommittedEventsClear();
    }
}