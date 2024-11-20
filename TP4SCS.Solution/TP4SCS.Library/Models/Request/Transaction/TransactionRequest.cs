using TP4SCS.Library.Models.Request.Payment;

namespace TP4SCS.Library.Models.Request.Transaction
{
    public class TransactionRequest
    {
        public int AccountId { get; set; }

        public int MethodId { get; set; }

        public int PackId { get; set; }

        public decimal Balance { get; set; }

        public DateTime ProcessTime { get; set; }

        public string? Description { get; set; }

        public PaymentOptions PaymentMethod { get; set; }

        public string Status { get; set; } = null!;
    }
}
