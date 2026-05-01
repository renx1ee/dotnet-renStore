using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreSize;

internal sealed class RestoreVariantSizeCommandHandler
    : IRequestHandler<RestoreVariantSizeCommand>
{
    private readonly ILogger<RestoreVariantSizeCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _userService;
    
    public RestoreVariantSizeCommandHandler(
        ILogger<RestoreVariantSizeCommandHandler> logger,
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
        RestoreVariantSizeCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId} and SizeId: {SizeId}",
            nameof(RestoreVariantSizeCommand),
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
        
        variant.RestoreSize(
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            now: DateTimeOffset.UtcNow, 
            sizeId: request.SizeId);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(RestoreVariantSizeCommand),
            request.VariantId,
            request.SizeId);
    }
}