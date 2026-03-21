using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsCreated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid DetailId,
    Guid VariantId,
    int CountryOfManufactureId,
    string Description,
    string Composition,
    string? ModelFeatures,
    string? DecorativeElements,
    string? Equipment,
    string? CaringOfThings,
    TypeOfPacking? TypeOfPackaging)
    : IDomainEvent;