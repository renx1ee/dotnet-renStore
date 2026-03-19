namespace RenStore.Catalog.Application.Features.Product.Commands.PublishProduct;

public sealed record PublishProductCommand(
    Guid ProductId,
    UserRole Role,
    Guid UserId) 
    : IRequest,
      ISellerProductCommand;