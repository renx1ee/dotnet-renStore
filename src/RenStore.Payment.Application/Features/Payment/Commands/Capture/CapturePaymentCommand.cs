using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.Capture;

public sealed record CapturePaymentCommand(
    Guid   PaymentId,
    string ExternalPaymentId) 
    : IRequest;