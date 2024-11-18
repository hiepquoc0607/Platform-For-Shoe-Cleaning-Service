namespace TP4SCS.Library.Models.Response.Payment
{
    public class VnPayResponse
    {
        public string OrderId { get; set; } = string.Empty;

        public string PaymentId { get; set; } = string.Empty;

        public string TransactionId { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;

        public string OrderDescription { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public string VnPayResponseCode { get; set; } = string.Empty;
    }
}
