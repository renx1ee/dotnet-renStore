using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Abstractions.Queries;

public interface IPaymentAttemptQuery
{
    Task<PaymentAttemptReadModel?> FindByIdAsync(
        Guid attemptId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<PaymentAttemptReadModel>> FindByPaymentIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken);
}