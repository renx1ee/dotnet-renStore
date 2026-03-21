namespace RenStore.Catalog.Application.Features.Product.Commands.ToDraft;

public sealed record DraftProductCommand(
    Guid ProductId) 
    : IRequest, 
      ISellerProductCommand; 