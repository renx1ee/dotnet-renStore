namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeStock;

public sealed record ChangeChangeStockProjectionCommand(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid SizeId,
    int InStock,
    int Sales)
    : IRequest;