using MediatR;
using RenStore.Payment.Domain.Enums;
using RenStore.SharedKernal.Domain.Enums;
using PaymentMethod = RenStore.Payment.Domain.Enums.PaymentMethod;

namespace RenStore.Payment.Application.Features.Payment.Commands.Create;

public sealed record CreatePaymentCommand(
    Guid            OrderId,
    Guid            CustomerId,
    decimal         Amount,
    Currency        Currency,
    PaymentProvider Provider,
    PaymentMethod   PaymentMethod) 
    : IRequest<Guid>;