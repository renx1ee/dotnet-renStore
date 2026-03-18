namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Archive;

public sealed record ArchiveProductVariantCommand(
    UserRole Role,
    Guid UserId,
    Guid VariantId) : IRequest;