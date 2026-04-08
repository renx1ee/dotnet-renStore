namespace RenStore.Catalog.Application.Features.Category.Queries.FindCategories;

public sealed record FindCategoriesQuery(
    uint Page,
    uint PageSize, 
    bool Descending,
    bool? IsDeleted = null,
    CategorySortBy SortBy = CategorySortBy.Id)
    : IRequest<PageResult<GetCategoryReadModel>>;