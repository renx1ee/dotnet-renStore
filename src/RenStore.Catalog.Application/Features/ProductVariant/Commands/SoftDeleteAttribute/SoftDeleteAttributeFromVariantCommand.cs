namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDeleteAttribute;

public sealed record SoftDeleteAttributeFromVariantCommand(
    Guid AttributeId,
    Guid VariantId)
    : IRequest,
      ISellerVariantCommand;