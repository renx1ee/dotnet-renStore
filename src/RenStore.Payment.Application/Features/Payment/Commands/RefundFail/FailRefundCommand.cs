using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.RefundFail;

public sealed record FailRefundCommand(
    Guid   PaymentId,
    Guid   RefundId,
    string Reason) 
    : IRequest;