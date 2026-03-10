using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.WebApi.Requests.Variant;

public sealed record AddSizeToVariantRequest(LetterSize LetterSize);