using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Abstractions.Queries;

public interface IRefundQuery
{
    Task<RefundReadModel?> FindByIdAsync(
        Guid refundId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<RefundReadModel>> FindByPaymentIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken);
}