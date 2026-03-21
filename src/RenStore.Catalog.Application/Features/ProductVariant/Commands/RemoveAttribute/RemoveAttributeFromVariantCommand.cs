namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveAttribute;

public sealed record RemoveAttributeFromVariantCommand(
    Guid AttributeId,
    Guid VariantId)
    : IRequest,
      ISellerVariantCommand;