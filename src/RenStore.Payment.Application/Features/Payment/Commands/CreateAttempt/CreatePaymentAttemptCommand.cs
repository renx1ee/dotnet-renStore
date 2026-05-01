using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.CreateAttempt;

public sealed record CreatePaymentAttemptCommand(
    Guid PaymentId) 
    : IRequest<Guid>;