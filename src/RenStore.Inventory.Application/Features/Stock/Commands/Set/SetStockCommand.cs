namespace RenStore.Inventory.Application.Features.Stock.Commands.Set;

public sealed record SetStockCommand(
    Guid StockId,
    int Count)
    : IRequest;