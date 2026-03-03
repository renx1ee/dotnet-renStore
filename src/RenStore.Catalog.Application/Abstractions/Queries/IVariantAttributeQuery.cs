using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IVariantAttributeQuery
{
    Task<IReadOnlyList<VariantAttributeReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        VariantAttributeSortBy sortBy = VariantAttributeSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);

    Task<VariantAttributeReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<VariantAttributeReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<VariantAttributeReadModel>> FindByVariantIdAsync(
        CancellationToken cancellationToken,
        Guid variantId,
        VariantAttributeSortBy sortBy = VariantAttributeSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);

    Task<IReadOnlyList<VariantAttributeReadModel>> FindByKeyAsync(
        CancellationToken cancellationToken,
        string key,
        VariantAttributeSortBy sortBy = VariantAttributeSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);
}