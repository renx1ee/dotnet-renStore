namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveSize;

public sealed record RemoveVariantSizeCommand(
    Guid VariantId,
    Guid SizeId)
    : IRequest,
      ISellerVariantCommand;