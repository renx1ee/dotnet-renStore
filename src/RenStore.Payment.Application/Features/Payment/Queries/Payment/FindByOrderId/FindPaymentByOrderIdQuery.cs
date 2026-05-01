using MediatR;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Payment.FindByOrderId;

public sealed record FindPaymentByOrderIdQuery(
    Guid OrderId) 
    : IRequest<PaymentReadModel?>;