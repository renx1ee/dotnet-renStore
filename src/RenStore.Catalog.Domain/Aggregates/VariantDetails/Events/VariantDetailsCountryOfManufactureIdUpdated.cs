using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsCountryOfManufactureIdUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    int CountryOfManufactureId)
    : IDomainEvent;