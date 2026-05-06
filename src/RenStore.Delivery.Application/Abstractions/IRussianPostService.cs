using RenStore.Delivery.Application.Requests;

namespace RenStore.Delivery.Application.Abstractions;

public interface IRussianPostService
{
    /// <summary>
    /// Создать отправление в Почте России.
    /// Возвращает трек-номер (barcode).
    /// </summary>
    Task<CreateRussianPostShipmentResult> CreateShipmentAsync(
        CreateRussianPostShipmentRequest request,
        CancellationToken                cancellationToken);

    /// <summary>
    /// Получить актуальный статус трекинга по трек-номеру.
    /// </summary>
    Task<RussianPostTrackingResult> GetTrackingAsync(
        string            trackingNumber,
        CancellationToken cancellationToken);

    /// <summary>
    /// Удалить черновик отправления (до отправки на почту).
    /// </summary>
    Task<bool> DeleteShipmentAsync(
        string            trackingNumber,
        CancellationToken cancellationToken);
}