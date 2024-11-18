using Microsoft.AspNetCore.Http;
using TP4SCS.Library.Models.Request.Payment;
using TP4SCS.Library.Models.Response.Payment;

namespace TP4SCS.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext httpContext, VnPayRequest vnPayRequest);

        VnPayResponse PaymentExecue(IQueryCollection collection);
    }
}
