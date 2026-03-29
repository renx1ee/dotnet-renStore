using RenStore.Inventory.Domain.Aggregates.Reservation;

namespace RenStore.Inventory.Domain.Interfaces.Repository;

public interface IReservationRepository
{
    Task<VariantReservation?> GetAsync(
        Guid reservationId,
        CancellationToken cancellationToken);

    Task SaveAsync(
        VariantReservation reservation,
        CancellationToken cancellationToken);
}