using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface ICategoryProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        CategoryReadModel category,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<CategoryReadModel> categories,
        CancellationToken cancellationToken);

    void Remove(CategoryReadModel category);

    void RemoveRange(IReadOnlyCollection<CategoryReadModel> categories);
}