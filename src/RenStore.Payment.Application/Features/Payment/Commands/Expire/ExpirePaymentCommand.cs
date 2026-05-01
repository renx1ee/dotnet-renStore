using MediatR;

namespace RenStore.Payment.Application.Features.Payment.Commands.Expire;

public sealed record ExpirePaymentCommand(Guid PaymentId) : IRequest;