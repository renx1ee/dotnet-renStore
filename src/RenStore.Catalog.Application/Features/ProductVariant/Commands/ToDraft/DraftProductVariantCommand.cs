namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ToDraft;

public sealed record DraftProductVariantCommand(
    UserRole Role,
    Guid UserId,
    Guid VariantId) 
    : IRequest;