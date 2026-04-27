namespace RenStore.Ordering.Contracts.Responses;

public record GetVariantPriceResponse(
    Guid CorrelationId,
    decimal PriceAmount,
    string Currency,
    string ProductNameSnapshot
);