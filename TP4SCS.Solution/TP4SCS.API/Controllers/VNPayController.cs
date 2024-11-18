using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.Payment;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IHttpContextAccessor _httpContext;

        public VnPayController(IVnPayService vnPayService, IHttpContextAccessor httpContext)
        {
            _vnPayService = vnPayService;
            _httpContext = httpContext;
        }

        [HttpPost]
        public IActionResult CreatePaymentUrlVnpay(VnPayRequest vnPayRequest)
        {
            var url = _vnPayService.CreatePaymentUrl(_httpContext.HttpContext!, vnPayRequest);

            return Redirect(url);
        }

        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecue(Request.Query);

            return Ok(response);
        }

    }
}
