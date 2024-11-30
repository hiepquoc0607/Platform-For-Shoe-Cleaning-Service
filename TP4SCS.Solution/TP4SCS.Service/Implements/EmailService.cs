using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using TP4SCS.Library.Utils.Healpers;
using TP4SCS.Services.Interfaces;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailOptions> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var retries = 3;
        var delay = TimeSpan.FromSeconds(2);
        while (retries > 0)
        {
            try
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress("ShoeCareHub", _emailSettings.SenderEmail));
                mailMessage.To.Add(new MailboxAddress($"{toEmail}", toEmail));
                mailMessage.Subject = subject;
                mailMessage.Body = new TextPart(TextFormat.Html) { Text = body };

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);

                    smtpClient.Authenticate(_emailSettings.SenderEmail, _emailSettings.SenderPassword);

                    smtpClient.Send(mailMessage);

                    smtpClient.Disconnect(true);
                }

                _logger.LogInformation($"Email sent successfully to {toEmail}.", toEmail);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending email.");
            }

            retries--;
            if (retries > 0)
            {
                _logger.LogWarning($"Retrying to send email. Remaining attempts: {retries}.", retries);
                await Task.Delay(delay);
            }
            else
            {
                _logger.LogError("Failed to send email after multiple attempts.");
            }
        }
    }
}
