using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.WebApi.Requests.Variant;

public record CreateVariantRequest(
    int ColorId,
    string Name,
    SizeSystem SizeSystem,
    SizeType SizeType);