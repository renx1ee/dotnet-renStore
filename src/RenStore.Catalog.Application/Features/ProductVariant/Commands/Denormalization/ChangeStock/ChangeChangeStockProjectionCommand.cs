namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeStock;

public sealed record ChangeChangeStockProjectionCommand(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int InStock)
    : IRequest;