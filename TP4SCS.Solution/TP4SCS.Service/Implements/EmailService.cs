using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks.Dataflow;
using TP4SCS.Library.Utils.Healpers;
using TP4SCS.Services.Interfaces;

public class EmailService : IEmailService, IDisposable
{
    private readonly EmailOptions _emailSettings;
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<EmailService> _logger;
    private bool _disposed = false;

    public EmailService(IOptions<EmailOptions> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;

        _smtpClient = new SmtpClient(_emailSettings.SmtpServer)
        {
            Port = _emailSettings.SmtpPort,
            Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
            EnableSsl = true,
            Timeout = 30000,  // 30 seconds timeout
        };
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _smtpClient.Dispose();
            _disposed = true;
        }
    }

    public async Task SendBatchEmailAsync(List<string> recipientEmails, string subject, string body, int batchSize = 10, int maxDegreeOfParallelism = 3)
    {
        // Set up a BatchBlock to create batches of emails with the specified batch size
        var batchBlock = new BatchBlock<string>(batchSize);

        // Set up an ActionBlock to process each batch asynchronously
        var actionBlock = new ActionBlock<string[]>(async batch =>
        {
            using var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                EnableSsl = true,
            };

            foreach (var recipient in batch)
            {
                try
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.SenderEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(recipient);

                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send to {recipient}: {ex.Message}");
                }
            }
        },
        new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        });

        // Post emails to batch block
        foreach (var email in recipientEmails)
        {
            batchBlock.Post(email);
        }

        batchBlock.Complete();

        // Link batch block to action block and ensure all batches are processed
        batchBlock.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });

        await actionBlock.Completion;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var retries = 3;
        var delay = TimeSpan.FromSeconds(2);
        while (retries > 0)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                await _smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent successfully to {toEmail}.", toEmail);
                return;
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, "SMTP error while sending email.");
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
