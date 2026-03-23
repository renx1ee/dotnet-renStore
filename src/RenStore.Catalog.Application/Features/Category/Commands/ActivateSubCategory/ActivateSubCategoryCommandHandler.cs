using RenStore.Catalog.Application.Features.Category.Commands.DeactivateSubCategory;

namespace RenStore.Catalog.Application.Features.Category.Commands.ActivateSubCategory;

internal sealed class ActivateSubCategoryCommandHandler
    : IRequestHandler<ActivateSubCategoryCommand>
{
    private readonly ILogger<DeactivateSubCategoryCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _userService;

    public ActivateSubCategoryCommandHandler(
        ILogger<DeactivateSubCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        ActivateSubCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with CategoryId: {CategoryId}, SubCategoryId: {SubCategoryId}",
            nameof(ActivateSubCategoryCommand),
            request.CategoryId,
            request.SubCategoryId);
        
        var category = await _categoryRepository
            .GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Category.Category),
                request.CategoryId);
        }
        
        category.ActivateSubCategory(
            subCategoryId: request.SubCategoryId,
            updatedById: _userService.UserId,
            updatedByRole: _userService.Role,
            now: DateTimeOffset.UtcNow);
        
        await _categoryRepository.SaveAsync(category, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. CategoryId: {CategoryId}, SubCategoryId: {SubCategoryId}",
            nameof(ActivateSubCategoryCommand),
            request.CategoryId,
            request.SubCategoryId);
    }
}