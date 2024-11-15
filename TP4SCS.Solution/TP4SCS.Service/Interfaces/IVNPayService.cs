using Microsoft.AspNetCore.Http;
using TP4SCS.Library.Models.Request.Payment;
using TP4SCS.Library.Models.Response.Payment;

namespace TP4SCS.Services.Interfaces
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(VNPayRequest model, HttpContext context);
        VNPayResponse PaymentExecute(IQueryCollection collections);
    }
}
