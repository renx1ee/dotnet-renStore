namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDelete;

internal sealed class SoftDeleteProductVariantCommandHandler
    : IRequestHandler<SoftDeleteProductVariantCommand>
{
    private readonly ILogger<SoftDeleteProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public SoftDeleteProductVariantCommandHandler(
        ILogger<SoftDeleteProductVariantCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        SoftDeleteProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(SoftDeleteProductVariantCommand),
            request.VariantId);
        
        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);

        variant.Delete(DateTimeOffset.UtcNow);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(SoftDeleteProductVariantCommand),
            request.VariantId);
    }
}