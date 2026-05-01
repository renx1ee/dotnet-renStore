using MediatR;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Refund.FindRefundsByPaymentId;

public sealed record FindRefundsByPaymentIdQuery(Guid PaymentId)
    : IRequest<IReadOnlyList<RefundReadModel>>;