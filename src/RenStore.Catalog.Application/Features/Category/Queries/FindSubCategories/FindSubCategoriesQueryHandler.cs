using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.ReadModels;
using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.Category.Queries.FindSubCategories;

internal sealed class FindSubCategoriesQueryHandler
    : IRequestHandler<FindSubCategoriesQuery, PageResult<GetSubCategoryReadModel>>
{
    private readonly ILogger<FindSubCategoriesQueryHandler> _logger;
    private readonly ICategoryQuery _categoryQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindSubCategoriesQueryHandler(
        ILogger<FindSubCategoriesQueryHandler> logger,
        ICategoryQuery categoryQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _categoryQuery = categoryQuery;
        _currentUserService = currentUserService;
    }

    public async Task<PageResult<GetSubCategoryReadModel>> Handle(
        FindSubCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with CategoryId: {CategoryId}.", 
            nameof(FindSubCategoriesQuery),
            request.CategoryId);

        bool? isDeleted =
            request.IsDeleted.HasValue
            && _currentUserService.Role is Roles.Admin 
                or Roles.Moderator 
                or Roles.Support 
                ? request.IsDeleted
                : false;

        var result = await _categoryQuery
            .FindSubCategoriesAsync(
                categoryId: request.CategoryId,
                page: request.Page,
                pageSize: request.PageSize,
                isDeleted: isDeleted,
                sortBy: request.SortBy,
                descending: request.Descending,
                cancellationToken: cancellationToken);
        
        _logger.LogInformation("{Query} handled. CategoryId: {CategoryId}", 
            nameof(FindSubCategoriesQuery),
            request.CategoryId);

        return result;
    }
}