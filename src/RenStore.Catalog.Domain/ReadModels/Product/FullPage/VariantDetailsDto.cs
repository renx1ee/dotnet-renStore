using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed record VariantDetailsDto(
    Guid DetailsId,
    string Description,
    string Composition,
    string? ModelFeatures,
    string? DecorativeElements,
    string? Equipment,
    string? CaringOfThings,
    TypeOfPacking? TypeOfPacking);