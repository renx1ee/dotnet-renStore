using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddSize;

public sealed record AddSizeToVariantCommand(
    Guid VariantId,
    LetterSize LetterSize) 
    : IRequest;