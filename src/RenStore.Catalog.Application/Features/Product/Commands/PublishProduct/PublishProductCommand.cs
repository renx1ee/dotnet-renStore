namespace RenStore.Catalog.Application.Features.Product.Commands.PublishProduct;

public sealed record PublishProductCommand(
    Guid ProductId) 
    : IRequest,
      ISellerProductCommand;