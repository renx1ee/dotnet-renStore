using RenStore.Catalog.Application.ReadModels;
using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.Category.Queries.FindCategories;

internal sealed class FindCategoriesQueryHandler  
    : IRequestHandler<FindCategoriesQuery, PageResult<GetCategoryReadModel>>
{
    private readonly ILogger<FindCategoriesQueryHandler> _logger;
    private readonly ICategoryQuery _categoryQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindCategoriesQueryHandler(
        ILogger<FindCategoriesQueryHandler> logger,
        ICategoryQuery categoryQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _categoryQuery = categoryQuery;
        _currentUserService = currentUserService;
    }

    public async Task<PageResult<GetCategoryReadModel>> Handle(
        FindCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {Query}.", nameof(FindCategoriesQuery));

        bool? isDeleted =
            request.IsDeleted.HasValue
            && _currentUserService.Role is Roles.Admin 
                                        or Roles.Moderator 
                                        or Roles.Support 
                ? request.IsDeleted
                : false;

        var result = await _categoryQuery
            .FindCategoriesAsync(
                page: request.Page,
                pageSize: request.PageSize,
                isDeleted: isDeleted,
                sortBy: request.SortBy,
                descending: request.Descending,
                cancellationToken: cancellationToken);
        
        _logger.LogInformation("{Query} handled.", nameof(FindCategoriesQuery));

        return result;
    }
}