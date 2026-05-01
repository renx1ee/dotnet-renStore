namespace RenStore.Payment.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<Domain.Aggregates.Payment.Payment?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task SaveAsync(
        Domain.Aggregates.Payment.Payment payment,
        CancellationToken cancellationToken);
}