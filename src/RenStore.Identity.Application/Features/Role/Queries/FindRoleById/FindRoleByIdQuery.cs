namespace RenStore.Identity.Application.Features.Role.Queries.FindRoleById;

public sealed record FindRoleByIdQuery(Guid RoleId) : IRequest<RoleReadModel?>;