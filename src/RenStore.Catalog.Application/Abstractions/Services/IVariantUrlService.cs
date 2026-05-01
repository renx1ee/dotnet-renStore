namespace RenStore.Catalog.Application.Abstractions.Services;

public interface IVariantUrlService
{
    string GenerateUrl(
        string name,
        long article);
}