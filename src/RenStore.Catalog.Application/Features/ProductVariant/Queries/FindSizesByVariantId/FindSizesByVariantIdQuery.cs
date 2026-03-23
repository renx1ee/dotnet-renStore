namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindSizesByVariantId;

public sealed record FindSizesByVariantIdQuery(
    Guid VariantId,
    VariantSizeSortBy SortBy = VariantSizeSortBy.Id,
    uint Page = 1,
    uint PageCount = 25,
    bool Descending = false,
    bool? IsDeleted = null)
    : IRequest<IReadOnlyList<VariantSizeReadModel>>;