namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddAttribute;

internal sealed class AddAttributeToVariantCommandHandler
    : IRequestHandler<AddAttributeToVariantCommand>
{
    private readonly ILogger<AddAttributeToVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public AddAttributeToVariantCommandHandler(
        ILogger<AddAttributeToVariantCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        AddAttributeToVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(AddAttributeToVariantCommand),
            request.VariantId);
        
        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);
        
        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);
        }
        
        variant.AddAttribute(
            now: DateTimeOffset.UtcNow, 
            key: request.Key,
            value: request.Value);
        
        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(AddAttributeToVariantCommand),
            request.VariantId);
    }
}