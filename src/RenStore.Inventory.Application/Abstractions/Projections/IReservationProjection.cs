using RenStore.Inventory.Domain.Enums;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Application.Abstractions.Projections;

public interface IReservationProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<Guid> AddAsync(
        VariantReservationReadModel reservation,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantReservationReadModel> reservations,
        CancellationToken cancellationToken);

    Task MarkAsCancelAsync(
        DateTimeOffset now,
        ReservationCancelReason reason,
        Guid reservationId,
        CancellationToken cancellationToken);

    Task MarkAsExpiredAsync(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken);

    Task MarkAsReleasedAsync(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken);

    Task MarkAsConfirmedAsync(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken);

    Task SoftDelete(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken);

    Task Restore(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken);

    void Remove(VariantReservationReadModel reservation);

    void RemoveRange(IReadOnlyCollection<VariantReservationReadModel> reservation);
}