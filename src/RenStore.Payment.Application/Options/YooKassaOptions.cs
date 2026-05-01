namespace RenStore.Payment.Application.Options;

public sealed class YooKassaOptions
{
    public string ShopId   { get; init; }  = null!;
    public string SecretKey { get; init; } = null!;
    public string BaseUrl   { get; init; } = "https://api.yookassa.ru/v3/";
    public string ReturnUrl { get; init; } = null!;
}
