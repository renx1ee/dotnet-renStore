using Microsoft.Extensions.Configuration;

namespace RenStore.Catalog.Application.Service;

public class LocalStorageService : IStorageService
{
    private readonly string _storagePath;
    private readonly string _baseUrl;

    public LocalStorageService(
        IConfiguration configuration)
    {
        _storagePath = configuration["Storage:Local"]!;
        _baseUrl = configuration["Storage:BaseUrl"]!;
        Directory.CreateDirectory(_baseUrl);
    }
    
    public async Task<string> UploadAsync(
        string fileName, 
        string contentType, 
        Stream stream, 
        CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_storagePath, fileName);

        if (stream.CanSeek)
            stream.Seek(0, SeekOrigin.Begin);
        
        await using var fileStream = File.Create(filePath);
        await stream.CopyToAsync(fileStream, cancellationToken);
        
        return filePath;
    }
    
    public async Task DeleteAsync(
        string fileName, 
        CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        
        if(File.Exists(filePath))
            File.Delete(filePath);
        
        await Task.CompletedTask;
    }
}