//using Microsoft.AspNetCore.Mvc;
//using TP4SCS.Library.Models.Request.Payment;
//using TP4SCS.Services.Interfaces;

//namespace TP4SCS.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class VNPayController : ControllerBase
//    {
//        private readonly IVNPayService _vnPayService;

//        public VNPayController(IVNPayService vnPayService)
//        {
//            _vnPayService = vnPayService;
//        }

//        [HttpPost]
//        public IActionResult CreatePaymentUrlVnpay(VNPayRequest model)
//        {
//            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

//            return Redirect(url);
//        }

//        [HttpGet]
//        public IActionResult PaymentCallbackVnpay()
//        {
//            var response = _vnPayService.PaymentExecute(Request.Query);

//            return Ok(response);
//        }

//    }
//}
