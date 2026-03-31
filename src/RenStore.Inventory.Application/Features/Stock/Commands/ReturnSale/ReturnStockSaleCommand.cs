namespace RenStore.Inventory.Application.Features.Stock.Commands.ReturnSale;

public sealed record ReturnStockSaleCommand(
    Guid StockId,
    int Count)
    : IRequest;