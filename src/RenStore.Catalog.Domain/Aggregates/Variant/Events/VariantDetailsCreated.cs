using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantDetailsCreated(
    DateTimeOffset OccurredAt,
    Guid Id,
    Guid VariantId,
    int CountryOfManufactureId,
    string Description,
    string Composition,
    string? ModelFeatures,
    string? DecorativeElements,
    string? Equipment,
    string? CaringOfThings,
    TypeOfPackaging? TypeOfPackaging);