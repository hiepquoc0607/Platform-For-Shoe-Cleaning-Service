using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Promotion
{
    public class PromotionUpdateRequest
    {
        public decimal NewPrice { get; set; }

        [DefaultValue("string")]
        public string Status { get; set; } = string.Empty;
    }
}
