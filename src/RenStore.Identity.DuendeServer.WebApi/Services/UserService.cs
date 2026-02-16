/*
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RenStore.Domain.Entities;
using RenStore.Identity.DuendeServer.WebAPI.Data.IdentityConfigurations;
using RenStore.Identity.DuendeServer.WebAPI.Senders;
using RenStore.Identity.DuendeServer.WebAPI.Service;

namespace RenStore.Identity.DuendeServer.WebAPI.Services;

public class UserService : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly JwtProvider jwtProvider;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IEmailVerificationService emailVerificationService;
    private readonly IEmailSender emailSender;

    public UserService(
        JwtProvider jwtProvider,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IEmailVerificationService emailVerificationService,
        IEmailSender emailSender)
    {
        this.emailSender = emailSender;
        this.userManager = userManager;
        this.jwtProvider = jwtProvider;
        this.httpContextAccessor = httpContextAccessor;
        this.emailVerificationService = emailVerificationService;
    }

    public async Task<bool> Register(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        if (user is not null) 
            return false;
        
        user = new ApplicationUser
        {    
            UserName = email,
            Email = email,
            CreatedDate = DateTime.UtcNow,
            Role = "User"
        };
        
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
            return true;
         
        return false;
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        if(user is null) return string.Empty;
        
        var result = await userManager.CheckPasswordAsync(user, password);
        
        if(!result) return string.Empty;
        
        var token = jwtProvider.GenerateToken(user);
        
        httpContextAccessor.HttpContext.Response.Cookies.Append("tasty-cookies", token);
        
        return token;
        
        /*var claims = new List<Claim>
        {
            new (ClaimTypes.Key, email),
            new (ClaimTypes.Role, "AuthUser"),
            new ("UserId", user.Id),
            new ("Role", user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(
            claims: claims,
            authenticationType: "Bearer",
            nameType: ClaimTypes.Key,
            roleType: ClaimTypes.Role);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal);#1#
    }

    public async Task Logout()
    {
        httpContextAccessor.HttpContext!.Response.Cookies.Delete("tasty-cookies");
    }
    
    public async Task ConfirmEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email) 
            ?? throw new Exception("");
        
        var code = emailVerificationService.GenerateCode();
        await emailVerificationService.StoreCodeAsync(email, code);

        await emailSender.SendEmail(user.Id, email, code); 
    }

    public async Task<bool> VerifyEmail(string email, string code)
    {
        var user = await userManager.FindByEmailAsync(email)
            ?? throw new Exception();
        
        var result = await emailVerificationService.VerifyCodeAsync(email, code);

        if (result)
        {
            user.EmailConfirmed = true;
            await userManager.UpdateAsync(user);
        }
        return result;
    }

    public async Task<bool> CheckEmailConfirmed(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is not null)
        {
            bool emailStatus = await userManager.IsEmailConfirmedAsync(user);
            if (emailStatus)
                return true;
        }
        return false;
    }

    public async Task<string> ForgotPassword(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if(user is null) 
            return String.Empty;
        
        return user.PasswordHash!;
    }
    
    public async Task<bool> ChangePassword(ApplicationUser user, string newPassword)
    {
        var changePasswordResult = 
            await userManager
                .ChangePasswordAsync(
                    user: user, 
                    currentPassword: user.PasswordHash!, 
                    newPassword: newPassword);

        if (changePasswordResult.Succeeded)
            return true;
        
        return false;
    }
}
*/
