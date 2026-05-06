using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Application.Abstractions.Services;

public interface ITokenService
{
    string GenerateAccessToken(ApplicationUserReadModel user);
    string GenerateRefreshToken();
}