namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Create;

public sealed record CreateProductVariantCommand(
    Guid ProductId,
    int ColorId,
    string Name,
    SizeSystem SizeSystem,
    SizeType SizeType) 
    : IRequest<CreateProductVariantResponse>,
      ISellerProductCommand;