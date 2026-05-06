namespace RenStore.Delivery.Application.Requests;

public sealed record CreateRussianPostShipmentRequest(
    string  RecipientName,
    string  RecipientPhone,
    string  AddressTo,
    string  PostcodeFrom,
    string  PostcodeTo,
    decimal WeightGrams,
    decimal ValueRub,     
    string  OrderId);