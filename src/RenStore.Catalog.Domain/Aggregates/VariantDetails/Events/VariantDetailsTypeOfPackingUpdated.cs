using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsTypeOfPackingUpdated(
    DateTimeOffset OccurredAt,
    TypeOfPacking TypeOfPacking)
    : IDomainEvent;