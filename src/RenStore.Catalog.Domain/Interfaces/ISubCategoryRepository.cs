using RenStore.Catalog.Domain.Aggregates.Category;

namespace RenStore.Catalog.Domain.Interfaces;

public interface ISubCategoryRepository
{
    Task<int> AddAsync(
        SubCategory subCategory,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<SubCategory> subCategories,
        CancellationToken cancellationToken);

    void Remove(SubCategory subCategory);

    void RemoveRange(IReadOnlyCollection<SubCategory> subCategories);
}