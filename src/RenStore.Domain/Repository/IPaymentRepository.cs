using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IPaymentRepository
{
    Task<Guid> CreateAsync(PaymentEntity payment, CancellationToken cancellationToken);

    Task UpdateAsync(PaymentEntity payment, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<PaymentEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        PaymentSortBy sortBy = PaymentSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<PaymentEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<PaymentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<PaymentEntity>> FindByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken,
        PaymentSortBy sortBy = PaymentSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<PaymentEntity>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken,
        PaymentSortBy sortBy = PaymentSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}