namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Create;

internal sealed class CreateProductVariantCommandHandler
    : IRequestHandler<CreateProductVariantCommand, Guid>
{
    private readonly ILogger<CreateProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IProductRepository _productRepository;
    
    
    public CreateProductVariantCommandHandler(
        ILogger<CreateProductVariantCommandHandler> logger,
        IProductRepository productRepository,
        IProductVariantRepository productVariantRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _productVariantRepository = productVariantRepository;
    }

    public async Task<Guid> Handle(
        CreateProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(CreateProductVariantCommand),
            request.ProductId);
        
        //TODO:  нужно убедиться, что SizeType согласован с категорией продукта,
        // иначе можно добавить несовместимый размер.
        
        // TODO: сделать проверки
        /*var belong = await _productProjection
            .BelongAsync(
                productId: request.ProductId,
                sellerId: sellerId,
                cancellationToken: cancellationToken);
        var existingProduct = await _productProjection
               .ExistsAsync(request.ProductId, cancellationToken);
           
           if (!existingProduct)
           {
               throw new NotFoundException(
                   name: typeof(Domain.Aggregates.Product.Product),
                   request.ProductId);
           }
        */
        
        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        
        Random rnd = new Random();
        var article = rnd.Next(1, 100000000); // TODO: сделать сервис для генерации артикула
        var url = Guid.NewGuid().ToString();      // TODO: сделать сервис для url

        var now = DateTimeOffset.UtcNow;

        var variant = Domain.Aggregates.Variant.ProductVariant
            .Create(
                now: now,
                productId: request.ProductId,
                colorId: request.ColorId,
                name: request.Name,
                sizeSystem: request.SizeSystem,
                sizeType: request.SizeType,
                article: article,
                url: url);
        
        product.AddVariantReference(variant.Id, now);
        
        await _productVariantRepository.SaveAsync(variant, cancellationToken);
        await _productRepository.SaveAsync(product, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(CreateProductVariantCommand),
            request.ProductId);

        return variant.Id;
    }
}