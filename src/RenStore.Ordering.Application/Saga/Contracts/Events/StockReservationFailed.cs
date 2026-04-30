namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record StockReservationFailed(
    Guid   CorrelationId,
    string Reason);