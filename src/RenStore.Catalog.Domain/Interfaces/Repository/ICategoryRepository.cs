using RenStore.Catalog.Domain.Aggregates.Category;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<Category?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task SaveAsync(
        Category category,
        CancellationToken cancellationToken);
}