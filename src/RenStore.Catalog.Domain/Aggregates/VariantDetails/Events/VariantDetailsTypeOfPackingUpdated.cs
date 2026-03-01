using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsTypeOfPackingUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    TypeOfPacking TypeOfPacking)
    : IDomainEvent;