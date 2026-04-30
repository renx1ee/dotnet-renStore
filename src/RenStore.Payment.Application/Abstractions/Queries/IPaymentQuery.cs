using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Abstractions.Queries;

public interface IPaymentQuery
{
    Task<PaymentReadModel?> FindByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken);

    Task<PaymentReadModel?> FindByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<PaymentReadModel>> FindByCustomerIdAsync(
        Guid             customerId,
        PaymentSortBy    sortBy = PaymentSortBy.CreatedAt,
        uint             page = 1,
        uint             pageSize = 25,
        bool             descending = true,
        PaymentStatus?   status = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PaymentReadModel>> FindAllAsync(
        PaymentSortBy    sortBy = PaymentSortBy.CreatedAt,
        uint             page = 1,
        uint             pageSize = 25,
        bool             descending = true,
        PaymentStatus?   status = null,
        CancellationToken cancellationToken = default);
}