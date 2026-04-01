namespace RenStore.Inventory.Application.Features.Stock.Commands.SoftDelete;

public sealed record StockSoftDeleteCommand(
    Guid VariantId,
    Guid SizeId)
    : IRequest;