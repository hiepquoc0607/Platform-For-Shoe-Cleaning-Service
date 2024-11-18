using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks.Dataflow;
using TP4SCS.Library.Utils.Healpers;
using TP4SCS.Services.Interfaces;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailSettings;
    private readonly SmtpClient _smtpClient;

    public EmailService(IOptions<EmailOptions> emailSettings)
    {
        _emailSettings = emailSettings.Value;
        _smtpClient = new SmtpClient(_emailSettings.SmtpServer)
        {
            Port = _emailSettings.SmtpPort,
            Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
            EnableSsl = true,
        };
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
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email sending failed: {ex.Message}");
        }
    }
}
