using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Abstractions.Projection;

public interface IPaymentAttemptProjection
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(
        PaymentAttemptReadModel attempt,
        CancellationToken cancellationToken);

    Task MarkAsSuccessfulAsync(
        DateTimeOffset now,
        Guid           attemptId,
        string?        externalAuthCode,
        CancellationToken cancellationToken);

    Task MarkAsFailedAsync(
        DateTimeOffset now,
        Guid           attemptId,
        string         failureReason,
        string?        errorCode,
        CancellationToken cancellationToken);
}