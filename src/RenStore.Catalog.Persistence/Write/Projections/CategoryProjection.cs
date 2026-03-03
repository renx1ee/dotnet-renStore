using RenStore.Catalog.Domain.Aggregates.Category;

namespace RenStore.Catalog.Persistence.Write.Projections;

public class CategoryProjection 
    : RenStore.Catalog.Application.Abstractions.Projections.ICategoryProjection
{
    private readonly CatalogDbContext _context;
    
    public CategoryProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
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