namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsCountryOfManufactureIdUpdated(
    DateTimeOffset OccurredAt,
    int CountryOfManufactureId,
    Guid VariantId);