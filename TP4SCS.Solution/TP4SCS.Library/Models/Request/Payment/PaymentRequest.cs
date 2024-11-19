namespace TP4SCS.Library.Models.Request.Payment
{
    public enum PaymentOptions
    {
        VnPay,
        ZaloPay
    }

    public class PaymentRequest
    {
        public int PackId { get; set; }

        public PaymentOptions Payment { get; set; }
    }
}
