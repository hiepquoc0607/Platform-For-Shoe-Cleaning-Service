using TP4SCS.Library.Models.Response.Account;

namespace TP4SCS.Library.Models.Response.Transaction
{
    public class TransactionResponse
    {
        public int Id { get; set; }

        public AccountResponse Account { get; set; } = new AccountResponse();

        public int PackId { get; set; }

        public decimal Balance { get; set; }

        public DateTime ProcessTime { get; set; }

        public string? Description { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
