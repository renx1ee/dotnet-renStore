namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindPriceHistoryBySizeId;

public sealed record FindPriceHistoryBySizeIdQuery(
    Guid VariantId,
    Guid SizeId,
    PriceHistorySortBy SortBy = PriceHistorySortBy.Id,
    uint Page = 1,
    uint PageCount = 25,
    bool Descending = false,
    bool? IsActive = null)
    : IRequest<IReadOnlyList<PriceHistoryReadModel>>;