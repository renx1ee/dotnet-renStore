namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SetMainImageId;

public sealed record SetVariantMainImageCommand(
    Guid UserId,
    Guid VariantId,
    Guid ImageId)
    : IRequest,
      ISellerVariantCommand;