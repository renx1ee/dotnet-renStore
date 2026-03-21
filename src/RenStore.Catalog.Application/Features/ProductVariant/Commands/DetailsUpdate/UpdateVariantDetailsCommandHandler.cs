namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.DetailsUpdate;

public class UpdateVariantDetailsCommandHandler
    : IRequestHandler<UpdateVariantDetailsCommand>
{
    private readonly ILogger<UpdateVariantDetailsCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    
    public UpdateVariantDetailsCommandHandler(
        ILogger<UpdateVariantDetailsCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        UpdateVariantDetailsCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(UpdateVariantDetailsCommand),
            request.VariantId);

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

        var now = DateTimeOffset.UtcNow;

        if (request.Description is not null)
            variant.ChangeDetailsDescription(now: now, description: request.Description);
        
        if (request.Composition is not null)
            variant.ChangeDetailsComposition(now: now, composition: request.Composition);
        
        if (request.ModelFeatures is not null)
            variant.ChangeDetailsModelFeatures(now: now, modelFeatures: request.ModelFeatures);
        
        if (request.DecorativeElements is not null)
            variant.ChangeDetailsDecorativeElements(now: now, decorativeElements: request.DecorativeElements);
        
        if (request.Equipment is not null)
            variant.ChangeDetailsEquipment(now: now, equipment: request.Equipment);
        
        if (request.CaringOfThings is not null)
            variant.ChangeDetailsCaringOfThings(now: now, caringOfThings: request.CaringOfThings);
        
        if (request.TypeOfPacking is not null)
            variant.ChangeDetailsTypeOfPacking(now: now, typeOfPacking: (TypeOfPacking)request.TypeOfPacking);

        if (!variant.GetUncommittedEvents().Any())
        {
            _logger.LogInformation(
                "No changes detected for VariantId: {VariantId}",
                request.VariantId);

            return;
        }

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(UpdateVariantDetailsCommand),
            request.VariantId);
    }
}