/*using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Identity;
using RenStore.Domain.Entities;
using RenStore.Identity.DuendeServer.WebAPI.DTOs;
using RenStore.Identity.DuendeServer.WebAPI.Service;
using RenStore.Identity.DuendeServer.WebAPI.Services;

namespace RenStore.Identity.DuendeServer.WebAPI.Endpoints;

public static class UserEndpoints 
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth/");
        
        group.MapPost("/register", Register);

        group.MapPost("/login", Login);

        group.MapPost("/logout", Logout).RequireAuthorization();
        
        group.MapPost("/send-code-email", ConfirmEmail);
        
        group.MapPost("/verify-code-email", VerifyEmail);

        group.MapGet("/check-confirmed-email", CheckConfirmEmail);

        group.MapPost("/forgot-password", ForgotPassword);

        group.MapPost("/change-password", ChangePassword);

        group.MapPost("/refresh-token", RefreshToken);
        
        group.MapGet("/me", GetMyInfo).RequireAuthorization();
        
        group.MapPost("/update-me", UpdateProfile);
        
        group.MapPost("/assign-role", AssignRole);
        
        group.MapPost("/remove-role", RemoveRole);
        
        group.MapGet("/user-email", GetUserByEmail);
        
        return group;
    }

    private static async Task<IResult> Register(
        RegisterUserRequest request,
        UserService userService,
        IEmailVerificationService emailVerificationService)
    {
        var result = await userService.Register(
            email: request.Email, 
            password: request.Password);

        if (result) return Results.Ok();
        
        return Results.BadRequest();
    }

    private static async Task<IResult> Login(
        LoginUserRequest request,
        UserService userService)
    {
        var token = await userService.Login(request.Email!, request.Password!);
        
        if(!token.IsNullOrEmpty())
            return Results.Ok(new { Token = token});
        
        return Results.BadRequest("Email or password is incorrect.");
    }
    
    private static async Task<IResult> Logout(
        UserService service)
    {
        await service.Logout();
        return Results.NoContent();
    }
    
    private static async Task<IResult> ConfirmEmail(
        string email,
        UserService userService)
    {
        await userService.ConfirmEmail(email);
        return Results.Ok();
    }
    
    private static async Task<IResult> VerifyEmail(
        VerifyEmailRequest request,
        UserService userService)
    {
        var result = await userService.VerifyEmail(request.Email, request.Code);
        if(result)
            return Results.Ok();
        
        return Results.BadRequest();
    }
    
    private static async Task<IResult> CheckConfirmEmail(
        string email,
        UserService userService)
    {
        var result = await userService.CheckEmailConfirmed(email);
        if(result)
            return Results.Ok();
        
        return Results.NoContent();
    }

    // TODO: доделать
    private static async Task<IResult> ForgotPassword()
    {
        
        // 1. send code
        // 2. check code
        // 3. create a new password
        return Results.Ok();
    }
    
    // TODO: доделать
    private static async Task<IResult> ChangePassword(
        ChangePasswordRequest request, 
        UserService service,
        HttpContext context,
        UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.GetUserAsync(context.User);
        if (user is null) 
            return Results.Unauthorized();
        
        var result = await service 
            .ChangePassword(user, request.NewPassword);
        
        return Results.Ok();
    }
    
    // TODO: доделать
    private static async Task<IResult> RefreshToken()
    {
        return Results.Ok();
    }

    // TODO: доделать
    private static async Task<IResult> GetMyInfo(
        UserManager<ApplicationUser> manager,
        HttpContext context)
    {
        var user = context.User;
        var jwtToken = context.Request.Cookies["tasty-cookies"];
        
        if (!context.User.Identity.IsAuthenticated)
            return Results.Unauthorized();
        
        return Results.Ok();
    }
    
    // TODO: доделать
    private static async Task<IResult> UpdateProfile()
    {
        return Results.Ok();
    }

    // TODO: доделать
    private static async Task<IResult> AssignRole()
    {
        return Results.Ok();
    }
    
    // TODO: доделать
    private static async Task<IResult> RemoveRole()
    {
        return Results.Ok();
    }

    // TODO: доделать
    private static async Task<IResult> GetUserByEmail()
    {
        return Results.Ok();
    }
}*/