namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface ICategoryQuery
{
    Task<GetCategoryReadModel?> FindCategoryAsync(
        Guid categoryId,
        bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    Task<PageResult<GetCategoryReadModel>> FindCategoriesAsync(
        CategorySortBy sortBy = CategorySortBy.Name,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);

    Task<PageResult<GetCategoryWithSubCategoryReadModel>> FindCategoriesWithSubCategoriesAsync(
        CategorySortBy sortBy = CategorySortBy.Name,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeletedCategory = null,
        bool? isDeletedSubCategory = null,
        CancellationToken cancellationToken = default);

    Task<GetSubCategoryReadModel?> FindSubCategoryAsync(
        Guid categoryId,
        Guid subCategoryId,
        bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    Task<PageResult<GetSubCategoryReadModel>> FindSubCategoriesAsync(
        Guid categoryId,
        SubCategorySortBy sortBy = SubCategorySortBy.Name,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);
}