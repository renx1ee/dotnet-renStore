using RenStore.Inventory.Domain.Constants;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Inventory.Domain.Aggregates.Reservation.Rules;

internal static class VariantReservationRules
{
    internal static void QuantityValidation(int quantity)
    {
        if (quantity is > InventoryConstants.VariantReservation.MaxInventoryReservationCount
                     or < InventoryConstants.VariantReservation.MinInventoryReservationCount)
        {
            throw new DomainException(
                $"Inventory reservation quantity must be between " +
                $"{InventoryConstants.VariantReservation.MinInventoryReservationCount} and " +
                $"{InventoryConstants.VariantReservation.MaxInventoryReservationCount}.");
        }
    }
    
    internal static void IdValidation(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException(
                "ID cannot be empty guid");
        }
    }
}