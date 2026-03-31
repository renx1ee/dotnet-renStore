namespace RenStore.Inventory.Application.Features.Stock.Commands.Restore;

public sealed record RestoreStockCommand(
    Guid StockId)
    : IRequest;