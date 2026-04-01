namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDeleteSize;

public sealed record SoftDeleteVariantSizeCommand(
    Guid VariantId,
    Guid SizeId)
    : IRequest,
      ISellerVariantCommand;