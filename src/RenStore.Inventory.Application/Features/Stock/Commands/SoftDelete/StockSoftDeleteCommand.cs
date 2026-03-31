namespace RenStore.Inventory.Application.Features.Stock.Commands.SoftDelete;

public sealed record StockSoftDeleteCommand(
    Guid StockId)
    : IRequest;