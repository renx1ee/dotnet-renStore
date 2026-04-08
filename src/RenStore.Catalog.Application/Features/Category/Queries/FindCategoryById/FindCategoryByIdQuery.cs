namespace RenStore.Catalog.Application.Features.Category.Queries.FindCategoryById;

public sealed record FindCategoryByIdQuery(
    Guid CategoryId,
    bool? IncludeDeleted = null)
    : IRequest<GetCategoryReadModel?>;