using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Deteils;

public sealed record VariantDetailsCountryOfManufactureIdUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    int CountryOfManufactureId)
    : IDomainEvent;