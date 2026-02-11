namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public record VariantDetailsCountryOfManufactureIdUpdated(
    DateTimeOffset OccurredAt,
    int CountryOfManufactureId,
    Guid VariantId);