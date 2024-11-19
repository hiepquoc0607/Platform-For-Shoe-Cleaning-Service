namespace TP4SCS.Library.Models.Response.Payment
{
    public class PaymentResponse
    {
        public int TransactionId { get; set; }

        public string PaymentId { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public string VnPayResponseCode { get; set; } = string.Empty;
    }
}
