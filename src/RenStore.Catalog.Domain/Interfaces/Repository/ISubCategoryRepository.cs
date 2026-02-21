using RenStore.Catalog.Domain.Aggregates.Category;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface ISubCategoryRepository
{
    Task<SubCategory?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        SubCategory subCategory,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<SubCategory> subCategories,
        CancellationToken cancellationToken);

    void Remove(SubCategory subCategory);

    void RemoveRange(IReadOnlyCollection<SubCategory> subCategories);
}