namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Create;

public sealed record CreateProductVariantResponse(
    Guid Id,
    long Article,
    string UrlSlug,
    string Name);