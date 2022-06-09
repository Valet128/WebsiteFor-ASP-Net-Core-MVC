using MailKit.Net.Smtp;
using MimeKit;

namespace ShvedovaAV.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string[] emails, string subject, string message)
        {
            foreach (var email in emails)
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("АШАШ", "email@mail.ru"));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = message
                };
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.mail.ru", 465, true);
                    await client.AuthenticateAsync("email@mail.ru", "password");
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
