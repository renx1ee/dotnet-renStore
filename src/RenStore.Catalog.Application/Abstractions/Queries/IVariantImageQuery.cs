namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IVariantImageQuery
{
    Task<IReadOnlyList<VariantImageReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        VariantImageSortBy sortBy = VariantImageSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        bool? isMain = null);

    Task<VariantImageReadModel?> FindByIdAsync(
        CancellationToken cancellationToken,
        Guid id);

    Task<VariantImageReadModel> GetByIdAsync(
        CancellationToken cancellationToken,
        Guid id);

    Task<IReadOnlyList<VariantImageReadModel>> FindByVariantIdAsync(
        CancellationToken cancellationToken,
        Guid variantId,
        VariantImageSortBy sortBy = VariantImageSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        bool? isMain = null);

    Task<int> GetNextSortOrderAsync(
        Guid variantId,
        CancellationToken cancellationToken);
}