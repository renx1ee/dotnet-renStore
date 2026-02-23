namespace RenStore.Identity.DuendeServer.WebAPI.Senders;

public interface IEmailSender
{
    Task SendEmail(string userId, string email, string value);
}