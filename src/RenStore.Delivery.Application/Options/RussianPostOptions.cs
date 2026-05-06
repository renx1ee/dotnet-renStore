namespace RenStore.Delivery.Application.Options;

public sealed class RussianPostOptions
{
    public string BaseUrl       { get; init; } = "https://otpravka-api.pochta.ru";
    public string Login         { get; init; } = null!;
    public string Password      { get; init; } = null!;
    public string AccessToken   { get; init; } = null!; // токен из ЛК Почты России
    public string TrackingUrl   { get; init; } = "https://tracking.pochta.ru/tracking/api";
    public string TrackingToken { get; init; } = null!;
}