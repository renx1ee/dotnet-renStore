namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveSize;

internal sealed class RemoveVariantSizeCommandHandler
    : IRequestHandler<RemoveVariantSizeCommand>
{
    private readonly ILogger<RemoveVariantSizeCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _userService;
    
    public RemoveVariantSizeCommandHandler(
        ILogger<RemoveVariantSizeCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IProductRepository productRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _productRepository = productRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        RemoveVariantSizeCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId} and SizeId: {SizeId}",
            nameof(AddSizeToVariantCommand),
            request.VariantId,
            request.SizeId);

        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);

        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.VariantId);
        }
        
        var product = await _productRepository
            .GetAsync(variant.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                variant.ProductId);
        }

        variant.RemoveSize(
            sizeId: request.SizeId,
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId,
            now: DateTimeOffset.UtcNow);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId} SizeId: {SizeId}",
            nameof(AddSizeToVariantCommand),
            request.VariantId,
            request.SizeId);
    }
}