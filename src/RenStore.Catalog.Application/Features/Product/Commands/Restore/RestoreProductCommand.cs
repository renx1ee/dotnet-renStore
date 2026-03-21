namespace RenStore.Catalog.Application.Features.Product.Commands.Restore;

public sealed record RestoreProductCommand(
    Guid ProductId) 
    : IRequest,
      ISellerProductCommand;