using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Category;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class CategoryRepository 
    : RenStore.Catalog.Domain.Interfaces.Repository.ICategoryRepository
{
    private readonly IEventStore _eventStore;
    
    public CategoryRepository(IEventStore eventStore) =>
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    
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
        // TODO:
    }
}