using Microsoft.AspNetCore.Identity.UI.Services;

namespace IvaETicaret.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // return  SenderEmail.Gonder(subject, htmlMessage, email);
            return null;
        }

    }
}
