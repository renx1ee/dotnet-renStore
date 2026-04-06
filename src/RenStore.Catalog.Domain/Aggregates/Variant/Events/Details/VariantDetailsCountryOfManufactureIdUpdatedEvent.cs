using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsCountryOfManufactureIdUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string CountryOfManufacture,
    Guid DetailId)
    : IDomainEvent;