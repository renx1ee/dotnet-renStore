using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public sealed record VariantDetailsTypeOfPackingUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    TypeOfPacking TypeOfPacking)
    : IDomainEvent;