using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.WebApi.Requests.Variant;

public sealed record AddVariantDetailsRequest(
    int CountryOfManufactureId,
    string Description,
    string Composition,
    string? CaringOfThings = null,
    TypeOfPacking? TypeOfPacking = null,
    string? ModelFeatures = null,
    string? DecorativeElements = null,
    string? Equipment = null);