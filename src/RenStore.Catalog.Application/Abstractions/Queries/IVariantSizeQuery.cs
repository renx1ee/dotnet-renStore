namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IVariantSizeQuery
{
    Task<IReadOnlyList<VariantSizeReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        VariantSizeSortBy sortBy = VariantSizeSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);

    Task<VariantSizeReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<VariantSizeReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<VariantSizeReadModel>> FindByVariantIdAsync(
        Guid variantId,
        CancellationToken cancellationToken,
        VariantSizeSortBy sortBy = VariantSizeSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);
}