namespace RenStore.Order.Application.Saga.Records;

public sealed record OrderPlacementCompleted(
    Guid CorrelationId,
    Guid OrderId) 
    : CorrelatedBy<Guid>;