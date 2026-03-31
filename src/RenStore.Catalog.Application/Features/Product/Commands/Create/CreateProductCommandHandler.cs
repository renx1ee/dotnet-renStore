using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

internal sealed class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ISubCategoryProjection _subCategoryProjection;
    private readonly ICurrentUserService _userService;
    
    public CreateProductCommandHandler(
        ILogger<CreateProductCommandHandler> logger,
        IProductRepository productRepository,
        ISubCategoryProjection subCategoryProjection,
        ICurrentUserService userService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _subCategoryProjection = subCategoryProjection;
        _userService = userService;
    }
    
    public async Task<Guid> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {CommandName} for SellerId {SellerId}, SubCategoryId: {SubCategoryId}", 
            nameof(CreateProductCommand),
            _userService.UserId,
            request.SubCategoryId);

        var existingSubCategory = await _subCategoryProjection
            .IsExistsAsync(
                categoryId: request.CategoryId, 
                subCategoryId: request.SubCategoryId, 
                cancellationToken);
        
        if (existingSubCategory == false)
        {
            throw new NotFoundException(
                name: typeof(SubCategoryReadModel), 
                request.SubCategoryId);
        }

        var product = Domain.Aggregates.Product.Product.Create(
            sellerId: _userService.UserId
                      ?? throw new UnauthorizedException(),
            subCategoryId: request.SubCategoryId,
            categoryId: request.CategoryId,
            now: DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(
            product, cancellationToken);

        _logger.LogInformation(
            "{CommandName} handled successfully. ProductId: {ProductId}, SubCategoryId: {SubCategoryId}",
            nameof(CreateProductCommand),
            product.Id,
            request.SubCategoryId);
        
        return product.Id;
    }
}