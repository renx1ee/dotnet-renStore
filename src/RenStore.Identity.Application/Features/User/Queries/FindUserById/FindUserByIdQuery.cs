namespace RenStore.Identity.Application.Features.User.Queries.FindUserById;

public sealed record FindUserByIdQuery(Guid UserId) : IRequest<ApplicationUserReadModel?>;