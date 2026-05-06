namespace RenStore.Identity.Application.Features.User.Commands.Login;

public sealed record LoginResult(
    Guid   UserId,
    string AccessToken,
    string RefreshToken);