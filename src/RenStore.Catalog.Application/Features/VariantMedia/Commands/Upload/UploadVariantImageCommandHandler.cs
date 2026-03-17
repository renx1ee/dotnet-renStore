using RenStore.Catalog.Application.Service;
using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Upload;

internal sealed class UploadVariantImageCommandHandler
    : IRequestHandler<UploadVariantImageCommand, Guid>
{
    private readonly ILogger<UploadVariantImageCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IVariantImageRepository _variantImageRepository;
    private readonly IStorageService _storageService;
    private readonly IVariantImageQuery _variantImageQuery; 

    public UploadVariantImageCommandHandler(
        ILogger<UploadVariantImageCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IVariantImageRepository variantImageRepository,
        IStorageService storageService,
        IVariantImageQuery variantImageQuery)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _variantImageRepository = variantImageRepository;
        _storageService = storageService;
        _variantImageQuery = variantImageQuery;
    }
    
    public async Task<Guid> Handle(
        UploadVariantImageCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(UploadVariantImageCommand),
            request.VariantId);

        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);

        if (variant is null)
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);
        
        var now = DateTimeOffset.UtcNow;
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.FileName)}";

        request.Stream.Seek(0, SeekOrigin.Begin);
        
        var url = await _storageService.UploadAsync(
            fileName: fileName,
            contentType: request.ContentType,
            stream: request.Stream,
            cancellationToken: cancellationToken);

        var nextSortOrder = await _variantImageQuery
            .GetNextSortOrderAsync(
                variantId: request.VariantId,
                cancellationToken: cancellationToken);
        
        var image = VariantImage.Create(
            now: now,
            variantId: request.VariantId,
            originalFileName: fileName,
            storagePath: url,
            fileSizeBytes: request.Stream.Length,
            isMain: !variant.ImageIds.Any(),
            sortOrder: nextSortOrder,
            weight: 100,   // TODO:
            height: 100);  // TODO:
        
        variant.AddImageReference(now: now, imageId: image.Id);
        
        await _variantImageRepository.SaveAsync(image, cancellationToken);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, ImageId: {ImageId}",
            nameof(UploadVariantImageCommand),
            request.VariantId,
            image.Id);

        return image.Id;
    }
}