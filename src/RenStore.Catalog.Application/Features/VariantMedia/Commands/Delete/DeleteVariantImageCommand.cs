namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Delete;

public sealed record DeleteVariantImageCommand(
    UserRole Role,
    Guid UserId,
    Guid VariantId,
    Guid ImageId) 
    : IRequest;