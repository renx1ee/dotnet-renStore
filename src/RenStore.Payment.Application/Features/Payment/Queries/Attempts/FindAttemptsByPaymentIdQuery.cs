using MediatR;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Queries.Attempts;

public sealed record FindAttemptsByPaymentIdQuery(Guid PaymentId)
    : IRequest<IReadOnlyList<PaymentAttemptReadModel>>;