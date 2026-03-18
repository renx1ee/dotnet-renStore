namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDelete;

public sealed record SoftDeleteProductVariantCommand(
    UserRole Role,
    Guid UserId,
    Guid VariantId) 
    : IRequest;