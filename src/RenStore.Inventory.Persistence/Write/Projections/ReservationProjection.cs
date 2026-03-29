using Microsoft.EntityFrameworkCore;
using RenStore.Inventory.Domain.Enums;
using RenStore.Inventory.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Inventory.Persistence.Write.Projections;

internal sealed class ReservationProjection
    : RenStore.Inventory.Application.Abstractions.Projections.IReservationProjection
{
    private readonly InventoryDbContext _context;

    public ReservationProjection(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> AddAsync(
        VariantReservationReadModel reservation,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        await _context.Reservations.AddAsync(reservation, cancellationToken);

        return reservation.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantReservationReadModel> reservations,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reservations);

        var reservationList = reservations as IList<VariantReservationReadModel> ?? reservations.ToList();

        if (reservationList.Count == 0) return;

        await _context.Reservations.AddRangeAsync(reservationList, cancellationToken);
    }

    public async Task MarkAsCancelAsync(
        DateTimeOffset now,
        ReservationCancelReason reason, 
        Guid reservationId,
        CancellationToken cancellationToken)
    {
        var reservation = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);
        
        reservation.UpdatedAt = now;
        reservation.Status = ReservationStatus.Cancelled;
        reservation.CancelReason = reason;
    }
    
    public async Task MarkAsExpiredAsync(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken)
    {
        var reservation = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);
        
        reservation.UpdatedAt = now;
        reservation.Status = ReservationStatus.Expired;
    }
    
    public async Task MarkAsReleasedAsync(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken)
    {
        var reservation = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);
        
        reservation.UpdatedAt = now;
        reservation.Status = ReservationStatus.Released;
    }
    
    public async Task MarkAsConfirmedAsync(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken)
    {
        var reservation = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);
        
        reservation.UpdatedAt = now;
        reservation.Status = ReservationStatus.Confirmed;
    }

    public async Task SoftDelete(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken)
    {
        var reservation = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);

        reservation.Status = ReservationStatus.Deleted;
        reservation.UpdatedAt = now;
        reservation.DeletedAt = now;
    }
    
    public async Task Restore(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken)
    {
        var reservation = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);

        reservation.Status = ReservationStatus.Confirmed;
        reservation.UpdatedAt = now;
        reservation.DeletedAt = null;
    }

    public void Remove(
        VariantReservationReadModel reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        _context.Reservations.Remove(reservation);
    }
    
    public void RemoveRange(
        IReadOnlyCollection<VariantReservationReadModel> reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        _context.Reservations.RemoveRange(reservation);
    }

    private async Task<VariantReservationReadModel> GetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _context.Reservations
            .FirstOrDefaultAsync(x =>
                    x.Id == id,
                cancellationToken);

        if (result is null)
        {
            throw new NotFoundException(
                name: typeof(VariantReservationReadModel), 
                id);
        }

        return result;
    }
}