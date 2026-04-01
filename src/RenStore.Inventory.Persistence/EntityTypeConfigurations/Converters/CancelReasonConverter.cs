using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RenStore.Inventory.Domain.Enums;
using RenStore.Inventory.Persistence.EntityTypeConfigurations.Conversions;

namespace RenStore.Inventory.Persistence.EntityTypeConfigurations.Converters;

internal sealed class CancelReasonConverter : ValueConverter<ReservationCancelReason?, string?>
{
    public CancelReasonConverter()
        : base(
            r => r == null 
                ? null
                : VariantReservationConversion.ReservationCancelReasonToDatabase(r),
            r => r == null 
                ? null
                : VariantReservationConversion.ReservationCancelReasonFromDatabase(r))
    {
    }
}