namespace RenStore.Catalog.Application.Features.Product.Commands.Hide;

public sealed record HideProductCommand(
    Guid ProductId,
    UserRole Role,
    Guid UserId)
    : IRequest;