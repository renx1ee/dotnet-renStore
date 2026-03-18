namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreSize;

public sealed record RestoreVariantSizeCommand(
    UserRole Role,
    Guid UserId,
    Guid VariantId, 
    Guid SizeId) 
    : IRequest;