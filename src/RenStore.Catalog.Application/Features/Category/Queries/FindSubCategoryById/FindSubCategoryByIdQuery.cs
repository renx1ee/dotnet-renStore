using RenStore.Catalog.Application.ReadModels;

namespace RenStore.Catalog.Application.Features.Category.Queries.FindSubCategoryById;

public sealed record FindSubCategoryByIdQuery(
    Guid CategoryId,
    Guid SubCategoryId,
    bool? IncludeDeleted = null)
    : IRequest<GetSubCategoryReadModel?>;