using RenStore.Catalog.Domain.Aggregates.Category;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface ICategoryProjection
{
    Task<Guid> AddAsync(
        Category category,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<Category> categories,
        CancellationToken cancellationToken);

    void Remove(Category category);

    void RemoveRange(IReadOnlyCollection<Category> categories);
}