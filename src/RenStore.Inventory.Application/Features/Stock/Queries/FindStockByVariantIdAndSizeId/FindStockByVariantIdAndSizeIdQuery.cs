namespace RenStore.Inventory.Application.Features.Stock.Queries.FindStockByVariantIdAndSizeId;

public sealed record FindStockByVariantIdAndSizeIdQuery(
    Guid VariantId,
    Guid SizeId,
    bool? IsDeleted = null)
    : IRequest<VariantStockDto?>;