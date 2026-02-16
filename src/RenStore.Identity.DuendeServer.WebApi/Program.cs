using RenStore.Identity.DuendeServer.WebAPI.Data.IdentityConfigurations;
using RenStore.Identity.DuendeServer.WebAPI.Service;
using RenStore.Identity.DuendeServer.WebAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RenStore.Identity.DuendeServer.WebAPI.Extensions;
using RenStore.Identity.DuendeServer.WebAPI.Senders;
using RenStore.Identity.DuendeServer.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetValue<string>("DefaultConnection");
builder.Configuration.AddJsonFile("notification-message.json", optional: false, reloadOnChange: false);

builder.Services.AddDbContext<AuthDbContext>(optoins =>
{
    optoins.UseNpgsql(connectionString);
});

builder.Services.AddApiAuthentication();

/*builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddDefaultTokenProviders();*/

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.,@-+";
    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(Configuration.ApiResources)
    .AddInMemoryIdentityResources(Configuration.IdentityResources)
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddInMemoryClients(Configuration.Clients)
    .AddDeveloperSigningCredential();

builder.Services.AddScoped<JwtProvider>();
/*builder.Services.AddScoped<UserService>();*/
builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();
builder.Services.AddScoped<ICacheSender, CacheSender>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
} 
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

/*using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var roles = new[] { "AuthUser", "Admin", "Manager" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var data = 
                await roleManager.CreateAsync(
                    new ApplicationRole
                    {
                        Name = role
                    });
        }
    }
}*/

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider
            .GetRequiredService<AuthDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception exception)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "An error occurred app initialization.");
    }
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.RoutePrefix = string.Empty;
    config.SwaggerEndpoint("swagger/v1/swagger.json", "Shop API");
});

/*app.MapUserEndpoints();*/

app.Run();