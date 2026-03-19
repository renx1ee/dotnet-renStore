namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.PublishVariant;

public sealed record PublishProductVariantCommand(
    Guid UserId,
    Guid VariantId) 
    : IRequest,
      ISellerVariantCommand;