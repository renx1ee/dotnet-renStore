using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Category.Commands.CreateSubCategory;

internal sealed class CreateSubCategoryCommandHandler
    : IRequestHandler<CreateSubCategoryCommand, Guid>
{
    private readonly ILogger<CreateSubCategoryCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _userService;

    public CreateSubCategoryCommandHandler(
        ILogger<CreateSubCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _userService = userService;
    }
    
    public async Task<Guid> Handle(
        CreateSubCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command}",
            nameof(CreateSubCategoryCommand));
        
        var category = await _categoryRepository
            .GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Category.Category),
                request.CategoryId);
        }

        var subCategoryId = category.CreateSubCategory(
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            updatedByRole: _userService.Role,
            now: DateTimeOffset.UtcNow, 
            name: request.Name,
            nameRu: request.NameRu,
            isActive: request.IsActive,
            description: request.Description);
        
        await _categoryRepository.SaveAsync(category, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. CategoryId: {CategoryId}, SubCategoryId: {SubCategoryId}",
            nameof(CreateSubCategoryCommand),
            category.Id,
            subCategoryId);

        return subCategoryId;
    }
}