using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.ReadModels;
using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.Category.Queries.FindSubCategoryById;

internal sealed class FindSubCategoryByIdQueryHandler
    : IRequestHandler<FindSubCategoryByIdQuery, GetSubCategoryReadModel?>
{
    private readonly ILogger<FindSubCategoryByIdQueryHandler> _logger;
    private readonly ICategoryQuery _categoryQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindSubCategoryByIdQueryHandler(
        ILogger<FindSubCategoryByIdQueryHandler> logger,
        ICategoryQuery categoryQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _categoryQuery = categoryQuery;
        _currentUserService = currentUserService;
    }
    
    public async Task<GetSubCategoryReadModel?> Handle(
        FindSubCategoryByIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with CategoryId: {CategoryId} and SubCategoryId: {SubCategoryId}",
            nameof(FindSubCategoryByIdQuery),
            request.CategoryId,
            request.SubCategoryId);

        bool includeDeleted =
            // ReSharper disable once SimplifyConditionalTernaryExpression
            request.IncludeDeleted.HasValue &&
            _currentUserService.Role is Roles.Admin 
                                     or Roles.Moderator 
                                     or Roles.Support
                ? (bool)request.IncludeDeleted
                : false;

        var result = await _categoryQuery
            .FindSubCategoryAsync(
                categoryId: request.CategoryId,
                subCategoryId: request.SubCategoryId,
                includeDeleted: includeDeleted,
                cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Query} handled. CategoryId: {CategoryId} and SubCategoryId: {SubCategoryId}",
            nameof(FindSubCategoryByIdQuery),
            request.CategoryId,
            request.SubCategoryId);

        return result;
    }
}