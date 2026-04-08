namespace RenStore.Catalog.Application.Features.Category.Queries.FindSubCategories;

public sealed record FindSubCategoriesQuery(
    Guid CategoryId,
    uint Page,
    uint PageSize, 
    bool Descending,
    bool? IsDeleted = null,
    SubCategorySortBy SortBy = SubCategorySortBy.Id)
    : IRequest<PageResult<GetSubCategoryReadModel>>;