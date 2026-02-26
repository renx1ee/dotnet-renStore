using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Domain.Interfaces.Repository;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class CategoryRepository
    : ICategoryRepository
{
    private readonly CatalogDbContext _context;
    private readonly IEventStore _eventStore;
    
    public CategoryRepository(
        CatalogDbContext context,
        IEventStore eventStore)
    {
        _context = context       ?? throw new ArgumentNullException(nameof(context));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
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
    
    public async Task<Guid> AddAsync(
        Category category,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(category);

        await _context.Categories.AddAsync(category, cancellationToken);

        return category.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<Category> categories,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(categories);

        var categoriesList = categories as IList<Category> ?? categories.ToList();

        if (categoriesList.Count == 0) return;

        await _context.Categories.AddRangeAsync(categoriesList, cancellationToken);
    }

    public void Remove(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        _context.Categories.Remove(category);
    }
    
    public void RemoveRange(IReadOnlyCollection<Category> categories)
    {
        ArgumentNullException.ThrowIfNull(categories);

        _context.Categories.RemoveRange(categories);
    }
}