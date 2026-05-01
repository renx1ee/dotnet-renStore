using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Category.Commands.Activate;

internal sealed class ActivateCategoryCommandHandler
    : IRequestHandler<ActivateCategoryCommand>
{
    private readonly ILogger<ActivateCategoryCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _userService;

    public ActivateCategoryCommandHandler(
        ILogger<ActivateCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        ActivateCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with CategoryId: {CategoryId}",
            nameof(ActivateCategoryCommand),
            request.CategoryId);
        
        var category = await _categoryRepository
            .GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Category.Category),
                request.CategoryId);
        }
        
        category.Activate(
            updatedById: _userService.UserId 
                         ?? throw new UnauthorizedException(),
            updatedByRole: _userService.Role,
            now: DateTimeOffset.UtcNow);
        
        await _categoryRepository.SaveAsync(category, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. CategoryId: {CategoryId}",
            nameof(ActivateCategoryCommand),
            request.CategoryId);
    }
}