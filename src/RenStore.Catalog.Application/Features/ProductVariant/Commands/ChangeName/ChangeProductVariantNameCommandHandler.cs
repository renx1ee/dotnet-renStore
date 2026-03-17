namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ChangeName;

internal sealed class ChangeProductVariantNameCommandHandler
    : IRequestHandler<ChangeProductVariantNameCommand>
{
    private readonly ILogger<ChangeProductVariantNameCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public ChangeProductVariantNameCommandHandler(
        ILogger<ChangeProductVariantNameCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        ChangeProductVariantNameCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(ChangeProductVariantNameCommand),
            request.VariantId);
        
        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);

        variant.ChangeName(
            now: DateTimeOffset.UtcNow,
            name: request.Name);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(ChangeProductVariantNameCommand),
            request.VariantId);
    }
}