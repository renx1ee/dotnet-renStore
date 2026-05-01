using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.InitiatePayment;

public sealed record InitiatePaymentCommand(
    Guid   PaymentId,
    string Description) 
    : IRequest<InitiatePaymentResult>;