using System.ComponentModel.DataAnnotations;

namespace RenStore.Domain.Enums;

public enum ReturnReason
{
    WrongSize = 0,
    WrongColor = 1,
    NotAsDescribed = 2,
    Defective = 3,
    Damaged = 4,
    WrongItem = 5,
    ChangedMind = 6,
    LateDelivery = 7,
    PoorQuality = 8,
    Other = 9
}