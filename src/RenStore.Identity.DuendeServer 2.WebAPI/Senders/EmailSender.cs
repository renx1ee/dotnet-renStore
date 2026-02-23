using RenStore.Identity.DuendeServer.WebAPI.Data;
using RenStore.Identity.DuendeServer.WebAPI.DTOs;
using RenStore.Identity.DuendeServer.WebAPI.Service;

namespace RenStore.Identity.DuendeServer.WebAPI.Senders;

public class EmailSender(HttpClient httpClient, IConfiguration configuration) : IEmailSender
{
    public async Task SendEmail(string userId, string email, string value)
    {
        httpClient.BaseAddress = new Uri(UrlConstants.NotificationMicroserviceUrl);
        try
        {
            var data = new ConfirmEmailRequest
            {
                UserId = userId,
                To = email,
                Subject = configuration.GetValue<string>("ConfirmEmail:Subject")!,
                Body = value + configuration.GetValue<string>("ConfirmEmail:Body")
            };
            
            var response = await httpClient.PostAsJsonAsync(
                UrlConstants.SendEmailUrl,
                data);
            
            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            // ignored
        }
    }
}