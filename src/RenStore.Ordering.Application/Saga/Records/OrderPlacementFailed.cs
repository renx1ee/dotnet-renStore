namespace RenStore.Order.Application.Saga.Records;

public sealed record OrderPlacementFailed(
    Guid CorrelationId,
    string Reason)
    : CorrelatedBy<Guid>;