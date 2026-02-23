using RenStore.Identity.DuendeServer.WebAPI.Helpers;
using RenStore.Identity.DuendeServer.WebAPI.Senders;
using RenStore.Identity.DuendeServer.WebAPI.Services;

namespace RenStore.Identity.DuendeServer.WebAPI.Service;

public class EmailVerificationService : IEmailVerificationService
{
    private readonly ICacheSender cacheSender;
    private readonly uint secondsExpiration = 300;

    public EmailVerificationService(ICacheSender cacheSender) =>
        this.cacheSender = cacheSender;
    
    public string GenerateCode()
    {
        var random = new Random();
        return random.Next(1000, 9999).ToString();
    }

    public async Task StoreCodeAsync(string email, string code)
    {
        await cacheSender.SetCacheAsync(
            key: CacheKeyHelper
                .CreateEmailVerificationKey(email), 
            value: code, 
            seconds: secondsExpiration);
    }

    public async Task<bool> VerifyCodeAsync(string email, string code)
    {
        var storedCode = 
            await cacheSender.GetCacheAsync(
                CacheKeyHelper.CreateEmailVerificationKey(email));

        if (!string.IsNullOrEmpty(storedCode))
            return storedCode == code;
        
        return false;
    }
}