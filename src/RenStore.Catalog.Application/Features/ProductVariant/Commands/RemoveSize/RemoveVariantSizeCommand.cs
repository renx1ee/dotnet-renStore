namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveSize;

public sealed record RemoveVariantSizeCommand(
    UserRole Role,
    Guid UserId,
    Guid VariantId,
    Guid SizeId)
    : IRequest;