using Npgsql;
using RenStore.Catalog.Application.Service;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Create;

internal sealed class CreateProductVariantCommandHandler
    : IRequestHandler<CreateProductVariantCommand, CreateProductVariantResponse>
{
    private readonly ILogger<CreateProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IProductRepository _productRepository;
    private readonly IColorRepository _colorRepository;
    private readonly IArticleService _articleService;
    private readonly IVariantUrlService _variantUrlService;
    
    public CreateProductVariantCommandHandler(
        ILogger<CreateProductVariantCommandHandler> logger,
        IProductRepository productRepository,
        IProductVariantRepository productVariantRepository,
        IColorRepository colorRepository,
        IArticleService articleService,
        IVariantUrlService variantUrlService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _productVariantRepository = productVariantRepository;
        _colorRepository = colorRepository;
        _articleService = articleService;
        _variantUrlService = variantUrlService;
    }

    public async Task<CreateProductVariantResponse> Handle(
        CreateProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(CreateProductVariantCommand),
            request.ProductId);

        const int maxAttempts = 10;
        int currentAttempt = 0;
        
        //TODO:  нужно убедиться, что SizeType согласован с категорией продукта,
        // иначе можно добавить несовместимый размер.

        /*var colorExists = await _colorRepository.IsExists(
            colorId: request.ColorId,
            cancellationToken: cancellationToken);

        if (!colorExists)
        {
            throw new NotFoundException(
                name: typeof(Color),
                request.ColorId);
        }*/

        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        }
        
        while (currentAttempt <= maxAttempts)
        {
            currentAttempt++;

            try
            {
                var now = DateTimeOffset.UtcNow;
                
                var article = await _articleService.GenerateAsync(cancellationToken);

                var url = _variantUrlService.GenerateUrl(
                    article: article,
                    name: request.Name);
            
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

                return new CreateProductVariantResponse(
                    Id: variant.Id,
                    Article: variant.Article,
                    UrlSlug: variant.Url,
                    Name: variant.Name);
            }
            catch (Exception e) 
                when (IsUniqueArticleViolation(e))
            {
                _logger.LogWarning(
                    exception: e,
                    message: "Article conflict ont attempt {Attempt}, retrying",
                    args: currentAttempt);

                await Task.Delay(10 * currentAttempt, cancellationToken);

                continue;
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e, 
                    "{Command} failed after attempts: {Attempts}",
                    nameof(CreateProductVariantCommand),
                    currentAttempt);
                
                throw;
            }
        }

        throw new BusinessException("Cannot create unique article.");
    }

    private bool IsUniqueArticleViolation(Exception ex)
    {
        if (ex is PostgresException pgEx && 
            (pgEx.SqlState == "23505" || 
             pgEx.Message.Contains("unique", StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return false;
    }
}