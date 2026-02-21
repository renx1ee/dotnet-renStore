using RenStore.Catalog.Domain.Aggregates.Category;

namespace RenStore.Catalog.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<int> AddAsync(
        Category category,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<Category> categories,
        CancellationToken cancellationToken);

    void Remove(Category category);

    void RemoveRange(IReadOnlyCollection<Category> categories);
}