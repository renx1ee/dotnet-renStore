using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.Fail;

public sealed record FailPaymentCommand(
    Guid    PaymentId,
    Guid    AttemptId,
    string  Reason,
    string? ProviderErrorCode = null)
    : IRequest;
    
    