namespace RenStore.Order.Application.Saga.Contracts.Commands;

public sealed record ProcessPaymentRequest(
    Guid    CorrelationId,
    Guid    OrderId,
    Guid    CustomerId,
    decimal Amount,
    string  Currency);