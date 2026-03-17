namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ToDraft;

internal sealed class DraftProductVariantCommandHandler
    : IRequestHandler<DraftProductVariantCommand>
{
    private readonly ILogger<DraftProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public DraftProductVariantCommandHandler(
        ILogger<DraftProductVariantCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        DraftProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            typeof(DraftProductVariantCommand),
            request.VariantId);

        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);

        variant.ToDraft(DateTimeOffset.UtcNow);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            typeof(DraftProductVariantCommand),
            request.VariantId);
    }
}