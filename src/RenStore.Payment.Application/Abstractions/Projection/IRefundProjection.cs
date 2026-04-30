using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Abstractions.Projection;

public interface IRefundProjection
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(
        RefundReadModel refund,
        CancellationToken cancellationToken);

    Task MarkAsSucceededAsync(
        DateTimeOffset now,
        Guid           refundId,
        string         externalRefundId,
        CancellationToken cancellationToken);

    Task MarkAsFailedAsync(
        DateTimeOffset now,
        Guid           refundId,
        string         reason,
        CancellationToken cancellationToken);
}