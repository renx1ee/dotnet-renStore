namespace RenStore.Catalog.Application.Service;

public interface IStorageService
{
    Task<string> UploadAsync(
        string fileName,
        string contentType,
        Stream stream,
        CancellationToken cancellationToken);
}