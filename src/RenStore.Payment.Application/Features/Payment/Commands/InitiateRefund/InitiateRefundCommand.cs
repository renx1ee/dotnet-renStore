using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.InitiateRefund;

public sealed record InitiateRefundCommand(
    Guid    PaymentId,
    decimal Amount,
    string  Reason) 
    : IRequest<Guid>;