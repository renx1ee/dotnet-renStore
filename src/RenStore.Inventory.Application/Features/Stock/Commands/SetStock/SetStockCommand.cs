namespace RenStore.Inventory.Application.Features.Stock.Commands.SetStock;

public sealed record SetStockCommand(
    Guid StockId,
    int Count)
    : IRequest;