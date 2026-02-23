/*using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace RenStore.Identity.DuendeServer.WebAPI.Data.IdentityConfigurations;

public static class Configuration
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope(
                name: AuthConstants.AUTH_IDENTITY_CLIENT_ID,
                displayName: AuthConstants.AUTH_IDENTITY_DISPLAY_NAME)
        };
        
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource(
                name: AuthConstants.AUTH_IDENTITY_CLIENT_ID,
                displayName: AuthConstants.AUTH_IDENTITY_DISPLAY_NAME,
                userClaims: [JwtClaimTypes.Name])
            {
                Scopes = { AuthConstants.AUTH_IDENTITY_CLIENT_ID }
            }
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = AuthConstants.AUTH_IDENTITY_CLIENT_ID,
                ClientName = AuthConstants.AUTH_IDENTITY_CLIENT_NAME,
                
                AllowedGrantTypes = GrantTypes.Code,
                AccessTokenType = AccessTokenType.Jwt,
                
                RequirePkce = true,
                RequireClientSecret = false,
                RequireConsent = false,
                
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AccessTokenLifetime = 3600,
                
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = false,
                UpdateAccessTokenClaimsOnRefresh = true,
                
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    AuthConstants.AUTH_IDENTITY_CLIENT_ID
                },
                
                RedirectUris =
                {
                    "http://.../signin-oidc",
                    "https://localhost:7226/Home/Index/",

                },
                
                AllowedCorsOrigins =
                {
                    "http://..."
                },
                
                PostLogoutRedirectUris =
                {
                    "http://.../signout-oidc",
                }
            }
        };
}*/