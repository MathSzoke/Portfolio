using MailKit.Net.Smtp;
using MimeKit;

namespace Infra.Email;

public class SmtpEmailSender
{
    public async Task SendAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Portfolio Chat", "noreply@mathszoke.com"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, false);
        await client.AuthenticateAsync("matheusszoke@gmail.com", "senha-ou-app-password");
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}