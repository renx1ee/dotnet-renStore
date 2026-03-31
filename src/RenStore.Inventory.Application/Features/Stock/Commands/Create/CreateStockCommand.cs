namespace RenStore.Inventory.Application.Features.Stock.Commands.Create;

public sealed record CreateStockCommand(
    Guid SizeId,
    Guid VariantId,
    int InitialStock)
    : IRequest<Guid>;