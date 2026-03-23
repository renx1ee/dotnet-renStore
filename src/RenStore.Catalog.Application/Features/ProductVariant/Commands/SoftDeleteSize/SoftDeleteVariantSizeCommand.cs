namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveSize;

public sealed record SoftDeleteVariantSizeCommand(
    Guid VariantId,
    Guid SizeId)
    : IRequest,
      ISellerVariantCommand;