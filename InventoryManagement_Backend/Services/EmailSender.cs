using InventoryManagement_Backend.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace InventoryManagement_Backend.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        public EmailSender(IOptions<SmtpSettings> settings)
        {
            _smtpSettings = settings.Value;
        }

        public async Task SendEmailAsync(string subject, string body, List<string> recipients)
        {
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                EnableSsl = _smtpSettings.UseSsl,
                Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password)
            };

            var mail = new MailMessage()
            {
                From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            recipients.ForEach(recipient =>
            {
                mail.To.Add(recipient);
            });

            await client.SendMailAsync(mail);
        }

    }
}
