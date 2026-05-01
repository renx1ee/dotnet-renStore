using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Abstractions.Projection;

public interface IPaymentProjection
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task<Guid> AddAsync(
        PaymentReadModel payment,
        CancellationToken cancellationToken);

    Task SetAuthorizedAsync(
        DateTimeOffset now,
        Guid           paymentId,
        string         externalPaymentId,
        CancellationToken cancellationToken);

    Task SetCapturedAsync(
        DateTimeOffset now,
        Guid           paymentId,
        CancellationToken cancellationToken);

    Task SetFailedAsync(
        DateTimeOffset now,
        Guid           paymentId,
        string         failureReason,
        CancellationToken cancellationToken);

    Task SetCancelledAsync(
        DateTimeOffset now,
        Guid           paymentId,
        string         reason,
        CancellationToken cancellationToken);

    Task SetExpiredAsync(
        DateTimeOffset now,
        Guid           paymentId,
        CancellationToken cancellationToken);

    Task UpdateRefundedAmountAsync(
        DateTimeOffset now,
        Guid           paymentId,
        decimal        refundedAmount,
        PaymentStatus  status,
        CancellationToken cancellationToken);
    
    Task SetLastAttemptIdAsync(
        DateTimeOffset now,
        Guid           paymentId,
        Guid           lastAttemptId,
        CancellationToken cancellationToken);
}