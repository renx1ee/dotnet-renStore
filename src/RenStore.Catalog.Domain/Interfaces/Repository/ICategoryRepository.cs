using RenStore.Catalog.Domain.Aggregates.Category;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<Category?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        Category category,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<Category> categories,
        CancellationToken cancellationToken);

    void Remove(Category category);

    void RemoveRange(IReadOnlyCollection<Category> categories);
}