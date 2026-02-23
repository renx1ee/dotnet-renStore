namespace RenStore.Identity.DuendeServer.WebAPI.DTOs;

public class ConfirmEmailRequest
{
    public string UserId { get; set; }
    public string To { get; set; }
    public string Subject { get; set; } 
    public string Body { get; set; }
}
