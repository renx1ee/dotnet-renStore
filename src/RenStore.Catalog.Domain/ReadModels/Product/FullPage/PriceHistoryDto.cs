using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed record PriceHistoryDto(
    Guid PriceId,
    decimal Amount,
    Currency Currency,
    Guid SizeId);