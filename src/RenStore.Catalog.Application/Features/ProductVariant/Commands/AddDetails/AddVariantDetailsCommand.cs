namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddDetails;

public sealed record AddVariantDetailsCommand(
    Guid VariantId,
    string CountryOfManufacture,
    string Description,
    string Composition,
    string? CaringOfThings = null,
    TypeOfPacking? TypeOfPacking = null,
    string? ModelFeatures = null,
    string? DecorativeElements = null,
    string? Equipment = null)
    : IRequest;