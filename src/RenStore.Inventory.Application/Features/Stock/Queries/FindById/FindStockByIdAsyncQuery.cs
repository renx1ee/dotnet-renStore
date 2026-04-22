namespace RenStore.Inventory.Application.Features.Stock.Queries.FindById;

public sealed record FindStockByIdAsyncQuery(
    Guid StockId,
    bool? IsDeleted = null)
    : IRequest<VariantStockDto?>;