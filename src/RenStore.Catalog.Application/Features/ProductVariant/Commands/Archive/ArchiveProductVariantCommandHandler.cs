namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Archive;

internal sealed class ArchiveProductVariantCommandHandler
    : IRequestHandler<ArchiveProductVariantCommand>
{
    private readonly ILogger<ArchiveProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _userService;

    public ArchiveProductVariantCommandHandler(
        ILogger<ArchiveProductVariantCommandHandler> logger,
        IProductVariantRepository productVariantRepository,
        IProductRepository productRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _productVariantRepository = productVariantRepository;
        _productRepository = productRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        ArchiveProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            typeof(ArchiveProductVariantCommand),
            request.VariantId);

        var variant = await _productVariantRepository
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
        
        variant.Archive(
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            now: DateTimeOffset.UtcNow);

        await _productVariantRepository.SaveAsync(variant, cancellationToken);
            
        _logger.LogInformation(
            "{Command} handled successfully. VariantId: {VariantId}",
            typeof(ArchiveProductVariantCommand),
            request.VariantId);
    }
}