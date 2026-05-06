using RenStore.Delivery.Application.Enums;

namespace RenStore.Delivery.Application.Requests;

public sealed record RussianPostTrackingResult(
    string                        TrackingNumber,
    string                        RawStatus,
    RussianPostDeliveryStatus     MappedStatus,
    string?                       CurrentLocation,
    DateTimeOffset                OccurredAt);