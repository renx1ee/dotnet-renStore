namespace RenStore.Catalog.Application.Features.Category.Commands.Deactivate;

internal sealed class DeactivateCategoryCommandHandler
    : IRequestHandler<DeactivateCategoryCommand>
{
    private readonly ILogger<DeactivateCategoryCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _userService;

    public DeactivateCategoryCommandHandler(
        ILogger<DeactivateCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        DeactivateCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with CategoryId: {CategoryId}",
            nameof(DeactivateCategoryCommand),
            request.CategoryId);
        
        var category = await _categoryRepository
            .GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Category.Category),
                request.CategoryId);
        }
        
        category.Deactivate(
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            updatedByRole: _userService.Role,
            now: DateTimeOffset.UtcNow);
        
        await _categoryRepository.SaveAsync(category, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. CategoryId: {CategoryId}",
            nameof(DeactivateCategoryCommand),
            request.CategoryId);
    }
}