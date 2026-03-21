namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.UpdateAttribute;

internal sealed class UpdateVariantAttributeCommandHandler
    : IRequestHandler<UpdateVariantAttributeCommand>
{
    private readonly ILogger<UpdateVariantAttributeCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public UpdateVariantAttributeCommandHandler(
        ILogger<UpdateVariantAttributeCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        UpdateVariantAttributeCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}, AttributeId: {AttributeId}",
            nameof(UpdateVariantAttributeCommand),
            request.VariantId,
            request.AttributeId);
        
        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);
        
        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);
        }
        
        if(request.Key is not null)
            variant.ChangeAttributeKey(
                now: DateTimeOffset.UtcNow,
                attributeId: request.AttributeId,
                key: request.Key);
        
        if(request.Value is not null)
            variant.ChangeAttributeValue(
                now: DateTimeOffset.UtcNow,
                attributeId: request.AttributeId,
                value: request.Value);

        if (!variant.GetUncommittedEvents().Any())
        {
            _logger.LogInformation(
                "No changes detected for VariantId: {VariantId}",
                request.VariantId);
            
            return;
        }
        
        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, AttributeId: {AttributeId}",
            nameof(UpdateVariantAttributeCommand),
            request.VariantId,
            request.AttributeId);
    }
}