namespace RenStore.Inventory.Application.Features.Stock.Commands.AddToStock;

public sealed record AddToStockCommand(
    Guid StockId,
    int Count)
    : IRequest;
    
    