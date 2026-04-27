namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindActiveSizePrice;

public sealed record FindActiveVariantSizePriceBySizeIdQuery(
    Guid VariantId,
    Guid SizeId)
    : IRequest<PriceHistoryReadModel?>;