using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Features.Category.Commands.DeactivateSubCategory;
using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Category.Commands.SoftDeleteSubCategory;

internal sealed class SoftDeleteSubCategoryCommandHandler
    : IRequestHandler<SoftDeleteSubCategoryCommand>
{
    private readonly ILogger<SoftDeleteSubCategoryCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _userService;

    public SoftDeleteSubCategoryCommandHandler(
        ILogger<SoftDeleteSubCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        SoftDeleteSubCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with CategoryId: {CategoryId}, SubCategoryId: {SubCategoryId}",
            nameof(SoftDeleteSubCategoryCommand),
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
        
        category.DeleteSubCategory(
            subCategoryId: request.SubCategoryId,
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            updatedByRole: _userService.Role,
            now: DateTimeOffset.UtcNow);
        
        await _categoryRepository.SaveAsync(category, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. CategoryId: {CategoryId}, SubCategoryId: {SubCategoryId}",
            nameof(SoftDeleteSubCategoryCommand),
            request.CategoryId,
            request.SubCategoryId);
    }
}