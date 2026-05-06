using RenStore.Identity.Domain.Enums;

namespace RenStore.Identity.Application.Features.User.Queries.FindAll;

public sealed record FindAllUsersQuery(
    uint                   Page = 1,
    uint                   PageSize = 25,
    bool                   Descending = true,
    ApplicationUserStatus? Status = null) : IRequest<IReadOnlyList<ApplicationUserReadModel>>;