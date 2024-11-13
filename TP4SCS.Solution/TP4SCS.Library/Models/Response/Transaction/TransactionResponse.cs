namespace TP4SCS.Library.Models.Response.Transaction
{
    public class TransactionResponse
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int MethodId { get; set; }

        public int PackId { get; set; }

        public decimal Balance { get; set; }

        public DateTime ProcessTime { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = null!;
    }
}
