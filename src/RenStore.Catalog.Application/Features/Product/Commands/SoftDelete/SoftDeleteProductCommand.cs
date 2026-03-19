namespace RenStore.Catalog.Application.Features.Product.Commands.SoftDelete;

public sealed record SoftDeleteProductCommand(
    Guid ProductId,
    UserRole Role,
    Guid UserId) 
    : IRequest, 
      ISellerProductCommand;