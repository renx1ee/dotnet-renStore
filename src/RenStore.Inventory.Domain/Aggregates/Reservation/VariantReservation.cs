using System.Data;
using RenStore.Inventory.Domain.Aggregates.Reservation.Events;
using RenStore.Inventory.Domain.Aggregates.Reservation.Rules;
using RenStore.Inventory.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Inventory.Domain.Aggregates.Reservation;

/// <summary>
/// Represents a variant reservation physical entity with lifecycle and invariants.
/// </summary>
public sealed class VariantReservation
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    public Guid Id { get; private set; }
    public int Quantity { get; private set; }
    public ReservationStatus Status { get; private set; }
    public ReservationCancelReason? CancelReason { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public Guid UpdatedById { get; private set; } 
    public string UpdatedByRole { get; private set; } 
    public Guid StockId { get; private set; }
    public Guid VariantId { get; private set; }
    public Guid SizeId { get; private set; }
    public Guid OrderId { get; private set; }

    private VariantReservation() { }

    public static VariantReservation Create(
        int quantity,
        DateTimeOffset now,
        DateTimeOffset expiresAt,
        Guid variantId,
        Guid sizeId,
        Guid orderId)
    {
        VariantReservationRules.QuantityValidation(quantity);
        VariantReservationRules.IdValidation(variantId);
        VariantReservationRules.IdValidation(sizeId);
        VariantReservationRules.IdValidation(orderId);
        
        var reservation = new VariantReservation();
        var reservationId = Guid.NewGuid();
        
        reservation.Raise(
            new VariantReservationCreatedEvent(
                EventId: Guid.NewGuid(),
                ReservationId: reservationId,
                Quantity: quantity,
                OccurredAt: now,
                ExpiresAt: expiresAt,
                VariantId: variantId,
                SizeId: sizeId,
                Status: ReservationStatus.Active,
                OrderId: orderId));

        return reservation;
    }

    public void MarkAsCancel(
        DateTimeOffset now,
        ReservationCancelReason reason)
    {
        EnsureNotDeleted();
        
        Raise(new VariantReservationCancelledEvent(
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            Status: ReservationStatus.Cancelled,
            CancelReason: reason));
    }
    
    public void MarkAsExpired(
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        Raise(new VariantReservationExpiredEvent(
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            Status: ReservationStatus.Expired));
    }
    
    public void MarkAsReleased(
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        Raise(new VariantReservationReleased(
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            Status: ReservationStatus.Released));
    }
    
    public void MarkAsConfirmed(
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        Raise(new VariantReservationConfirmed(
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            Status: ReservationStatus.Confirmed));
    }
    
    public void Delete(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        Raise(new VariantReservationSoftDeleted(
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            ReservationId: Id,
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole));
    }
    
    public void Restore(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        if (Status != ReservationStatus.Deleted)
        {
            throw new DomainException(
                "An undeleted object cannot be recovered.");
        }
        
        Raise(new VariantReservationRestored(
            EventId: Guid.NewGuid(),
            OccurredAt: now,
            ReservationId: Id,
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole));
    }
    
    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case VariantReservationCreatedEvent e:
                Id = e.ReservationId;
                VariantId = e.VariantId;
                SizeId = e.SizeId;
                OrderId = e.OrderId;
                Quantity = e.Quantity;
                CreatedAt = e.OccurredAt;
                ExpiresAt = e.ExpiresAt;
                Status = e.Status;
                break;
            
            case VariantReservationCancelledEvent e:
                UpdatedAt = e.OccurredAt;
                Status = e.Status;
                CancelReason = e.CancelReason;
                break;
            
            case VariantReservationExpiredEvent e:
                UpdatedAt = e.OccurredAt;
                Status = e.Status;
                break;
            
            case VariantReservationReleased e:
                UpdatedAt = e.OccurredAt;
                Status = e.Status;
                break;
            
            case VariantReservationConfirmed e:
                UpdatedAt = e.OccurredAt;
                Status = e.Status;
                break;
            
            case VariantReservationSoftDeleted e:
                DeletedAt = e.OccurredAt;
                UpdatedAt = e.OccurredAt;
                Status = ReservationStatus.Deleted;
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                break;
            
            case VariantReservationRestored e:
                DeletedAt = null;
                UpdatedAt = e.OccurredAt;
                Status = ReservationStatus.Active;
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                break;
        }
    }

    public static VariantReservation Rehydrate(
        IReadOnlyList<IDomainEvent> events)
    {
        var reservation = new VariantReservation();
        
        foreach (var @event in events)
        {
            reservation.Apply(@event);
            reservation.Version++;
        }

        return reservation;
    }
    
    private void EnsureNotDeleted(string? message = null)
    {
        if (Status == ReservationStatus.Deleted)
        {
            throw new DomainException(
                message ?? "Entity is deleted.");
        }
    }
}
