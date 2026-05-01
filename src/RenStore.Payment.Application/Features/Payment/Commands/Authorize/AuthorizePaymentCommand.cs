using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.Authorize;

public sealed record AuthorizePaymentCommand(
    Guid    PaymentId,
    Guid    AttemptId,
    string  ExternalPaymentId,
    string? ExternalAuthCode = null) 
    : IRequest;