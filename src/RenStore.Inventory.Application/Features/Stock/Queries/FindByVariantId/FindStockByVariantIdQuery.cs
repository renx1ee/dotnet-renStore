namespace RenStore.Inventory.Application.Features.Stock.Queries.FindByVariantId;

public sealed record FindStockByVariantIdQuery(
    Guid VariantId,
    bool? IsDeleted = null)
    : IRequest<IReadOnlyList<VariantStockDto>>;