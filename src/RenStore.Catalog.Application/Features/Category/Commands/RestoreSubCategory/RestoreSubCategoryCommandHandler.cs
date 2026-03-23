namespace RenStore.Catalog.Application.Features.Category.Commands.RestoreSubCategory;

internal sealed class RestoreSubCategoryCommandHandler
    : IRequestHandler<RestoreSubCategoryCommand>
{
    private readonly ILogger<RestoreSubCategoryCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _userService;

    public RestoreSubCategoryCommandHandler(
        ILogger<RestoreSubCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        RestoreSubCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with CategoryId: {CategoryId}, SubCategoryId: {SubCategoryId}",
            nameof(RestoreSubCategoryCommand),
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
        
        category.RestoreSubCategory(
            subCategoryId: request.SubCategoryId,
            updatedById: _userService.UserId,
            updatedByRole: _userService.Role,
            now: DateTimeOffset.UtcNow);
        
        await _categoryRepository.SaveAsync(category, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. CategoryId: {CategoryId}, SubCategoryId: {SubCategoryId}",
            nameof(RestoreSubCategoryCommand),
            request.CategoryId,
            request.SubCategoryId);
    }
}