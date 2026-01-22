using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RenStore.Identity.DuendeServer.WebAPI.Data.IdentityConfigurations;

namespace RenStore.Identity.DuendeServer.WebAPI.Extensions;

public static class AuthExtensions
{
    public static void AddApiAuthentication(
        this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(AuthOptions.KEY))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context => 
                    {
                        context.Token = context.Request.Cookies["tasty-cookies"];
                        return Task.CompletedTask;
                    },
                };
            });
        /*.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
       {
           options.Events = new CookieAuthenticationEvents
           {
               OnRedirectToLogin = context =>
               {
                   context.Response.StatusCode = 401;
                   return Task.CompletedTask;
               },
               OnRedirectToAccessDenied = context =>
               {
                   context.Response.StatusCode = 403;
                   return Task.CompletedTask;
               }
           };
           options.ExpireTimeSpan = TimeSpan.FromMinutes(30000);
           options.Cookie.Key = "tasty-cookies";
       })*/
        services.AddAuthorization(options =>
        {
            /*options.AddPolicy("AuthUser", new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.Role, "AuthUser")
                .Build());
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });
            options.AddPolicy("Moderator", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Moderator");
            });*/
        });
    }
}