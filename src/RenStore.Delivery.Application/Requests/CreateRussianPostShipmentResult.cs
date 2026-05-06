namespace RenStore.Delivery.Application.Requests;

public sealed record CreateRussianPostShipmentResult(
    string  TrackingNumber,  // barcode — главный идентификатор
    bool    IsSucceeded,
    string? ErrorMessage = null);