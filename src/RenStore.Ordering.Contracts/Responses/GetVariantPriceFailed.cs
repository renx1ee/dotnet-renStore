namespace RenStore.Ordering.Contracts.Responses;

public record GetVariantPriceFailed(
    Guid CorrelationId,
    string Reason
);