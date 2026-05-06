namespace RenStore.Delivery.WebApi.Requests;

public sealed record ChangeTariffPriceRequest(
    decimal PriceAmount,
    string  Currency);