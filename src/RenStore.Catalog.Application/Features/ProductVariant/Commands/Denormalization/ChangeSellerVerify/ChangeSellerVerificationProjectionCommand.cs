namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeSellerVerify;

public sealed record ChangeSellerVerificationProjectionCommand(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    bool IsVerified)
    : IRequest;