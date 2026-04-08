namespace RenStore.Catalog.Application.Features.Category.Queries.FindCategoriesWithSubcategories;

public sealed record FindCategoriesWithSubcategoriesQuery(
    uint Page,
    uint PageSize, 
    bool Descending,
    bool? IsDeletedCategory = null,
    bool? IsDeletedSubCategory = null,
    CategorySortBy SortBy = CategorySortBy.Id)
    : IRequest<PageResult<GetCategoryWithSubCategoryReadModel>>;