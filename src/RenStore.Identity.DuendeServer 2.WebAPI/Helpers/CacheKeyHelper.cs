namespace RenStore.Identity.DuendeServer.WebAPI.Helpers;

public static class CacheKeyHelper
{
    private const string DefaultEmailKeyName = "EmailVerificationCode_";

    public static string CreateEmailVerificationKey(string key)
    {
        return DefaultEmailKeyName + key;
    }
}