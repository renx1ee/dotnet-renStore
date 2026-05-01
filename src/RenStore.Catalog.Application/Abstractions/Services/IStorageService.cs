namespace RenStore.Catalog.Application.Abstractions.Services;

public interface IStorageService
{
    Task<string> UploadAsync(
        string fileName,
        string contentType,
        Stream stream,
        CancellationToken cancellationToken);
}