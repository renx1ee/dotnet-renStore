namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.UpdateDetails;

public sealed record UpdateVariantDetailsCommand(
    Guid VariantId,
    string? Description = null,
    string? Composition = null,
    string? ModelFeatures = null,
    string? DecorativeElements = null,
    string? Equipment = null,
    string? CaringOfThings = null,
    TypeOfPacking? TypeOfPacking = null)
    : IRequest,
      ISellerVariantCommand;