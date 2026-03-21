namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddAttribute;

public sealed record AddAttributeToVariantCommand(
    Guid VariantId,
    string Key,
    string Value)
    : IRequest,
      ISellerVariantCommand;
