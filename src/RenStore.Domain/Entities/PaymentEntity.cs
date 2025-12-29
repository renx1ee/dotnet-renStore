using RenStore.Domain.Enums;
using RenStore.Microservice.Payment.Enums;

namespace RenStore.Domain.Entities;

public class PaymentEntity
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal Commission { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal? RefundedAmount { get; set; }
    public Currency Currency { get; set; } = Currency.RUB;
    public bool? IsSuccess { get; set; } = null;
    public PaymentMethod Method { get; set; }
    public string? MethodDetails { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string ErrorCode { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? AuthorizedDate { get; set; } // Дата авторизации
    public DateTime? CapturedDate { get; set; } // Дата подтверждения
    public DateTime? RefundedDate { get; set; }
    public DateTime? FailedDate { get; set; }
    public DateTime? ExpiryDate { get; set; } // Срок действия платежа
    public Guid OrderId { get; set; }
    public OrderEntity? Order { get; set; }
}

