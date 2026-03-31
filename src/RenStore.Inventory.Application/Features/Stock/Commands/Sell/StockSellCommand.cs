namespace RenStore.Inventory.Application.Features.Stock.Commands.Sell;

public sealed record StockSellCommand(
    Guid StockId,
    int Count)
    : IRequest;