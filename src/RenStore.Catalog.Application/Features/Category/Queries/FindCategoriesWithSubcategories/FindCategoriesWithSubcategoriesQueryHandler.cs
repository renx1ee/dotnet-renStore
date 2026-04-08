using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.Category.Queries.FindCategoriesWithSubcategories;

internal sealed class FindCategoriesWithSubcategoriesQueryHandler
    : IRequestHandler<FindCategoriesWithSubcategoriesQuery, PageResult<GetCategoryWithSubCategoryReadModel>>
{
    private readonly ILogger<FindCategoriesWithSubcategoriesQueryHandler> _logger;
    private readonly ICategoryQuery _categoryQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindCategoriesWithSubcategoriesQueryHandler(
        ILogger<FindCategoriesWithSubcategoriesQueryHandler> logger,
        ICategoryQuery categoryQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _categoryQuery = categoryQuery;
        _currentUserService = currentUserService;
    }

    public async Task<PageResult<GetCategoryWithSubCategoryReadModel>> Handle(
        FindCategoriesWithSubcategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {Query}.", nameof(FindCategoriesWithSubcategoriesQuery));

        bool? isDeletedCategory =
            request.IsDeletedCategory.HasValue
            && _currentUserService.Role is Roles.Admin 
                                        or Roles.Moderator 
                                        or Roles.Support 
                ? request.IsDeletedCategory
                : false;
        
        bool? isDeletedSubCategory =
            request.IsDeletedSubCategory.HasValue
            && _currentUserService.Role is Roles.Admin 
                                        or Roles.Moderator 
                                        or Roles.Support 
                ? request.IsDeletedCategory
                : false;

        var result = await _categoryQuery
            .FindCategoriesWithSubCategoriesAsync(
                page: request.Page,
                pageSize: request.PageSize,
                isDeletedCategory: isDeletedCategory,
                isDeletedSubCategory: isDeletedSubCategory,
                sortBy: request.SortBy,
                descending: request.Descending,
                cancellationToken: cancellationToken);
        
        _logger.LogInformation("{Query} handled.", nameof(FindCategoriesWithSubcategoriesQuery));

        return result;
    }
}