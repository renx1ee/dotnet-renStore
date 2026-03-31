using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Category.Commands.SoftDelete;

internal sealed class SoftDeleteCategoryCommandHandler
    : IRequestHandler<SoftDeleteCategoryCommand>
{
    private readonly ILogger<SoftDeleteCategoryCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _userService;

    public SoftDeleteCategoryCommandHandler(
        ILogger<SoftDeleteCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        SoftDeleteCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with CategoryId: {CategoryId}",
            nameof(SoftDeleteCategoryCommand),
            request.CategoryId);
        
        var category = await _categoryRepository
            .GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Category.Category),
                request.CategoryId);
        }
        
        category.Delete(
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            updatedByRole: _userService.Role,
            now: DateTimeOffset.UtcNow);
        
        await _categoryRepository.SaveAsync(category, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. CategoryId: {CategoryId}",
            nameof(SoftDeleteCategoryCommand),
            request.CategoryId);
    }
}