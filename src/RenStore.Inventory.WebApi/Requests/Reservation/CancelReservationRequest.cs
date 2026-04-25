using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.WebApi.Requests.Reservation;

public sealed record CancelReservationRequest(ReservationCancelReason Reason);