using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.ReadModels;
using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.Category.Queries.FindCategoryById;

internal sealed class FindCategoryByIdQueryHandler
    : IRequestHandler<FindCategoryByIdQuery, GetCategoryReadModel?>
{
    private readonly ILogger<FindCategoryByIdQueryHandler> _logger;
    private readonly ICategoryQuery _categoryQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindCategoryByIdQueryHandler(
        ILogger<FindCategoryByIdQueryHandler> logger,
        ICategoryQuery categoryQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _categoryQuery = categoryQuery;
        _currentUserService = currentUserService;
    }
    
    public async Task<GetCategoryReadModel?> Handle(
        FindCategoryByIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with CategoryId: {CategoryId}",
            nameof(FindCategoryByIdQuery),
            request.CategoryId);

        bool includeDeleted =
            // ReSharper disable once SimplifyConditionalTernaryExpression
            request.IncludeDeleted.HasValue &&
            _currentUserService.Role is Roles.Admin 
                                     or Roles.Moderator 
                                     or Roles.Support
                ? (bool)request.IncludeDeleted
                : false;
        
        var result = await _categoryQuery
            .FindCategoryAsync(
                categoryId: request.CategoryId,
                includeDeleted: includeDeleted,
                cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Query} handled. CategoryId: {CategoryId}",
            nameof(FindCategoryByIdQuery),
            request.CategoryId);

        return result;
    }
}