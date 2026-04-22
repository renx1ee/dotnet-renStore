namespace RenStore.Order.Application.Saga.Records;

public sealed record SagaTimeout(
    Guid CorrelationId) 
    : CorrelatedBy<Guid>;