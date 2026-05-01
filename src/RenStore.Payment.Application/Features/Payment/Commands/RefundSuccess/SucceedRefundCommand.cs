using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.RefundSuccess;

public sealed record SucceedRefundCommand(
    Guid   PaymentId,
    Guid   RefundId,
    string ExternalRefundId) 
    : IRequest;