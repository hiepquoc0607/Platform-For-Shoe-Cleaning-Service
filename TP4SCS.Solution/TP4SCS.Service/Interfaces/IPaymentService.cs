using Microsoft.AspNetCore.Http;
using TP4SCS.Library.Models.Request.Payment;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Payment;

namespace TP4SCS.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ApiResponse<string?>> CreatePaymentUrl(HttpContext httpContext, int id, PaymentRequest paymentRequest);

        Task<ApiResponse<PaymentResponse>> VnPayExcuteAsync(IQueryCollection collection);

        Task<ApiResponse<PaymentResponse>> ZaloPayExcuteAsync(IQueryCollection collection);
    }
}
