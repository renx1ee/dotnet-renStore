using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Category.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler
    : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _userService;

    public UpdateCategoryCommandHandler(
        ILogger<UpdateCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        UpdateCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with CategoryId: {CategoryId}",
            nameof(UpdateCategoryCommand),
            request.CategoryId);

        var category = await _categoryRepository
            .GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Category.Category),
                request.CategoryId);
        }
        
        var now = DateTimeOffset.UtcNow;

        var userId = _userService.UserId ?? throw new UnauthorizedException();

        if (request.Name is not null)
        {
            category.ChangeName(
                updatedById: userId,
                updatedByRole: _userService.Role,
                name: request.Name,
                now: now);
        }

        if (request.NameRu is not null)
        {
            category.ChangeNameRu(
                updatedById: userId,
                updatedByRole: _userService.Role,
                nameRu: request.NameRu,
                now: now);
        }

        if (request.Description is not null)
        {
            category.ChangeDescription(
                updatedById: userId,
                updatedByRole: _userService.Role,
                description: request.Description,
                now: now);
        }

        if (!category.GetUncommittedEvents().Any())
        {
            _logger.LogInformation(
                "No changes detected for CategoryId: {CategoryId}",
                request.CategoryId);
            
            return;
        }

        await _categoryRepository.SaveAsync(category, cancellationToken);
        
        _logger.LogInformation(
            "Handling {Command} with CategoryId: {CategoryId}",
            nameof(UpdateCategoryCommand),
            request.CategoryId);
    }
}