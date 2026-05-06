namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.RegisterWithRussianPost;

public sealed record RegisterWithRussianPostCommand(
    Guid    DeliveryOrderId,
    string  RecipientName,
    string  RecipientPhone,
    string  AddressTo,
    string  PostcodeFrom,
    string  PostcodeTo,
    decimal WeightGrams,
    decimal ValueRub) 
    : IRequest<string>;