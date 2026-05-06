namespace RenStore.Identity.Application.Options;

public sealed class JwtOptions
{
    public string SecretKey                { get; init; } = null!;
    public string Issuer                   { get; init; } = null!;
    public string Audience                 { get; init; } = null!;
    public int    AccessTokenExpiryMinutes { get; init; } = 60;
}