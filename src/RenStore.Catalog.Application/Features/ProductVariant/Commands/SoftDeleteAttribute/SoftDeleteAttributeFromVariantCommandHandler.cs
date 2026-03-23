using RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDeleteAttribute;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveAttribute;

internal sealed class SoftDeleteAttributeFromVariantCommandHandler
    : IRequestHandler<SoftDeleteAttributeFromVariantCommand>
{
    private readonly ILogger<SoftDeleteAttributeFromVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly ICurrentUserService _userService;
    
    public SoftDeleteAttributeFromVariantCommandHandler(
        ILogger<SoftDeleteAttributeFromVariantCommandHandler> logger,
        IProductVariantRepository variantRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        SoftDeleteAttributeFromVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}, AttributeId: {AttributeId}",
            nameof(SoftDeleteAttributeFromVariantCommand),
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
        
        variant.RemoveAttribute(
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId,
            now: DateTimeOffset.UtcNow,
            attributeId: request.AttributeId);
        
        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, AttributeId: {AttributeId}",
            nameof(SoftDeleteAttributeFromVariantCommand),
            request.VariantId,
            request.AttributeId);
    }
}