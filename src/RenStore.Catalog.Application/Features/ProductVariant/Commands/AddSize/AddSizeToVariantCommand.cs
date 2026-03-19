namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddSize;

public sealed record AddSizeToVariantCommand(
    Guid UserId,
    Guid VariantId,
    LetterSize LetterSize) 
    : IRequest,
      ISellerVariantCommand;