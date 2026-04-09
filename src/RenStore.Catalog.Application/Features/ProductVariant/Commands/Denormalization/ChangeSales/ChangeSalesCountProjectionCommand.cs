namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeSales;

public sealed record ChangeSalesCountProjectionCommand(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int Sales)
    : IRequest;