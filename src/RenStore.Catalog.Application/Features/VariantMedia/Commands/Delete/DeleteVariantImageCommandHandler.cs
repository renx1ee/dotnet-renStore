using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Delete;

internal sealed class DeleteVariantImageCommandHandler
    : IRequestHandler<DeleteVariantImageCommand>
{
    private readonly ILogger<DeleteVariantImageCommandHandler> _logger;
    private readonly IVariantImageRepository _variantImageRepository;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IProductRepository _productRepository;
    
    public DeleteVariantImageCommandHandler(
        ILogger<DeleteVariantImageCommandHandler> logger,
        IVariantImageRepository variantImageRepository,
        IProductVariantRepository productVariantRepository,
        IProductRepository productRepository)
    {
        _variantImageRepository = variantImageRepository;
        _logger = logger;
        _productVariantRepository = productVariantRepository;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        DeleteVariantImageCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ImageId: {ImageId}",
            nameof(DeleteVariantImageCommand),
            request.ImageId);
        
        var image = await _variantImageRepository.GetAsync(
            id: request.ImageId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(VariantImage),
                request.ImageId);
        
        var variant = await _productVariantRepository
            .GetAsync(request.VariantId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.VariantId);

        var product = await _productRepository
            .GetAsync(id: variant.ProductId, cancellationToken) 
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.VariantId);
        
        if (request.Role == UserRole.Seller &&
            product.SellerId != request.UserId)
        {
            throw new DomainException(nameof(request.UserId));
        }

        if (image.IsMain)
            throw new DomainException("Cannot delete main image. Set another image as main first.");
        
        image.Delete(
            updatedByRole: request.Role.ToString(),
            updatedById: request.UserId,
            now: DateTimeOffset.UtcNow);

        await _variantImageRepository.SaveAsync(image, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ImageId: {ImageId}",
            nameof(DeleteVariantImageCommand),
            request.ImageId);
    }
}