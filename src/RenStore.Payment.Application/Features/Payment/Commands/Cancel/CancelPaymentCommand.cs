using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.Cancel;

public sealed record CancelPaymentCommand(
    Guid PaymentId, 
    string Reason) 
    : IRequest;