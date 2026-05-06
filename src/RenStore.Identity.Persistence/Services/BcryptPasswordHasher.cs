using RenStore.Identity.Application.Abstractions.Services;

namespace RenStore.Identity.Persistence.Services;

internal sealed class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

    public bool Verify(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(password, hash);
}