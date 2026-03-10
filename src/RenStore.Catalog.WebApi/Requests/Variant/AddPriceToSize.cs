using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.WebApi.Requests.Variant;

public sealed record AddPriceToSize(
    Currency Currency,
    DateTimeOffset ValidFrom,
    decimal Price);