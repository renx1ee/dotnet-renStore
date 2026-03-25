namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreAttribute;

internal sealed class RestoreAttributeFromVariantCommandHandler
    : IRequestHandler<RestoreAttributeFromVariantCommand>
{
    private readonly ILogger<RestoreAttributeFromVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly ICurrentUserService _userService;
    
    public RestoreAttributeFromVariantCommandHandler(
        ILogger<RestoreAttributeFromVariantCommandHandler> logger,
        IProductVariantRepository variantRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        RestoreAttributeFromVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}, AttributeId: {AttributeId}",
            nameof(RestoreAttributeFromVariantCommand),
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
        
        variant.RestoreAttribute(
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            now: DateTimeOffset.UtcNow,
            attributeId: request.AttributeId);
        
        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, AttributeId: {AttributeId}",
            nameof(RestoreAttributeFromVariantCommand),
            request.VariantId,
            request.AttributeId);
    }
}