namespace TP4SCS.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);

        Task SendBatchEmailAsync(List<string> recipientEmails, string subject, string body, int batchSize = 10, int maxDegreeOfParallelism = 3);
    }
}
