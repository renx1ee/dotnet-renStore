using System.ComponentModel.DataAnnotations;

namespace RenStore.Identity.DuendeServer.WebAPI.DTOs;

public record ChangePasswordRequest(
    [Required] string CurrentPassword,
    [Required] string NewPassword
);