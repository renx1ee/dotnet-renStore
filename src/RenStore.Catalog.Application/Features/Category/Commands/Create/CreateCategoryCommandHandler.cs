namespace RenStore.Catalog.Application.Features.Category.Commands.Create;

internal sealed class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly ILogger<CreateCategoryCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryProjection _categoryProjection;
    private readonly ICurrentUserService _userService;

    public CreateCategoryCommandHandler(
        ILogger<CreateCategoryCommandHandler> logger,
        ICategoryRepository categoryRepository,
        ICategoryProjection categoryProjection,
        ICurrentUserService userService)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _categoryProjection = categoryProjection;
        _userService = userService;
    }
    
    public async Task<Guid> Handle(
        CreateCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command}",
            nameof(CreateCategoryCommand));
        
        var result = await _categoryProjection
            .IsExistsAsync(
                name: request.Name,
                nameRu: request.NameRu,
                cancellationToken: cancellationToken);
        
        if (result)
        {
            throw new DomainException(
                "Category already exists.");
        }

        var category = Domain.Aggregates.Category.Category.Create(
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
            "{Command} handled. CategoryId: {CategoryId}",
            nameof(CreateCategoryCommand),
            category.Id);

        return category.Id;
    }
}