namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddPrice;

public sealed record AddPriceToVariantSizeCommand(
    Guid UserId,
    Guid VariantId,
    Guid SizeId,
    Currency Currency,
    DateTimeOffset ValidFrom,
    decimal Price) 
    : IRequest,
      ISellerVariantCommand;