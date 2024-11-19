namespace TP4SCS.Library.Models.Request.SubscriptionPack
{
    public enum PaymentOptions
    {
        VnPay,
        ZaloPay
    }

    public class PackRegisterRequest
    {
        public int PackId { get; set; }

        public PaymentOptions Payment { get; set; }
    }
}
