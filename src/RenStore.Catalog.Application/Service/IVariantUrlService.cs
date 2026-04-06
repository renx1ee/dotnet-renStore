namespace RenStore.Catalog.Application.Service;

public interface IVariantUrlService
{
    string GenerateUrl(
        string name,
        long article);
}