namespace RenStore.Catalog.Application.Features.Product.Commands.Archive;

public sealed record ArchiveProductCommand(
    Guid ProductId,
    UserRole Role,
    Guid UserId) 
    : IRequest;