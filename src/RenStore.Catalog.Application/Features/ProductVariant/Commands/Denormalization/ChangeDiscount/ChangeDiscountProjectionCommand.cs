namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeDiscount;

public sealed record ChangeDiscountProjectionCommand(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int DiscountPercents)
    : IRequest;