namespace RenStore.Ordering.Contracts.Requests;

public sealed record GetVariantSizePriceRequest(
    Guid CorrelationId,
    Guid VariantId,
    Guid SizeId);