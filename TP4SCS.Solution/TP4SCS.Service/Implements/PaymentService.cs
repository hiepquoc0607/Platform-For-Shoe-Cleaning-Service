using Microsoft.AspNetCore.Http;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Payment;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Payment;
using TP4SCS.Library.Repositories;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class PaymentService : IPaymentService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly ISubscriptionPackRepository _subscriptionPackRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IVnPayService _vnPayService;

        public PaymentService(IBusinessRepository businessRepository,
            ISubscriptionPackRepository subscriptionPackRepository,
            ITransactionRepository transactionRepository,
            IVnPayService vnPayService)
        {
            _businessRepository = businessRepository;
            _subscriptionPackRepository = subscriptionPackRepository;
            _transactionRepository = transactionRepository;
            _vnPayService = vnPayService;
        }

        public async Task<ApiResponse<string?>> CreatePaymentUrlAsync(HttpContext httpContext, int id, PaymentRequest paymentRequest)
        {
            if (!paymentRequest.Payment.ToString().Equals(PaymentOptions.VnPay.ToString()) && !paymentRequest.Payment.ToString().Equals(PaymentOptions.ZaloPay.ToString()))
            {
                return new ApiResponse<string?>("error", 400, "Phương Thức Thanh Toán Không Hợp Lệ!");
            }

            var business = await _businessRepository.GetBusinessByOwnerIdAsync(id);

            if (business == null)
            {
                return new ApiResponse<string?>("error", 404, "Không Tìm Thấy Thông Tin Doanh Nghiệp!");
            }

            var pack = await _subscriptionPackRepository.GetPackByIdAsync(paymentRequest.PackId);

            if (pack == null)
            {
                return new ApiResponse<string?>("error", 404, "Không Tìm Thấy Thông Tin Gói Đăng Kí!");
            }

            var newTransaction = new Transaction
            {
                AccountId = id,
                PackId = paymentRequest.PackId,
                Balance = pack.Price,
                ProcessTime = DateTime.Now,
                Description = "Thanh Toán " + pack.Name + " Bằng " + paymentRequest.Payment,
                Status = StatusConstants.PENDING
            };

            var vnpay = new VnPayRequest
            {
                TransactionId = pack.Id,
                Balance = (double)newTransaction.Balance,
                CreatedDate = DateTime.Now,
                Description = newTransaction.Description,
            };

            string payUrl = "";

            try
            {
                await _subscriptionPackRepository.RunInTransactionAsync(async () =>
                {
                    await _transactionRepository.CreateTransactionAsync(newTransaction);

                    if (paymentRequest.Payment.Equals(PaymentOptions.VnPay))
                    {
                        payUrl = await _vnPayService.CreatePaymentUrlAsync(httpContext, vnpay);
                    }
                    else
                    {
                        payUrl = await _vnPayService.CreatePaymentUrlAsync(httpContext, vnpay);
                    }

                    newTransaction.Status = StatusConstants.PROCESSING;

                    await _transactionRepository.UpdateTransactionAsync(newTransaction);
                });

                return new ApiResponse<string?>("success", "Tạo Đường Dẫn Thanh Toán Gói Đăng Kí Thành Công!", payUrl, 201);
            }
            catch (Exception)
            {
                return new ApiResponse<string?>("error", 400, "Tạo Đường Dẫn Thanh Toán Gói Đăng Kí Thất Bại!");
            }
        }

        public async Task<ApiResponse<PaymentResponse>> VnPayExcuteAsync(IQueryCollection collection)
        {
            var result = await _vnPayService.PaymentExecuteAsync(collection);

            var transaction = await _transactionRepository.GetTransactionByIdAsync(result.TransactionId);

            if (transaction == null)
            {
                return new ApiResponse<PaymentResponse>("error", 404, "Không Tìm Thấy Thông Tin Giao Dịch!");
            }

            var business = await _businessRepository.GetBusinessByOwnerIdAsync(transaction.AccountId);

            if (business == null)
            {
                return new ApiResponse<PaymentResponse>("error", 404, "Không Tìm Thấy Thông Tin Doanh Nghiệp!");
            }

            var pack = await _subscriptionPackRepository.GetPackByIdAsync(transaction.PackId);

            if (pack == null)
            {
                return new ApiResponse<PaymentResponse>("error", 404, "Không Tìm Thấy Thông Tin Gói Đăng Kí!");
            }

            if (result.IsSuccess)
            {
                transaction.Status = StatusConstants.COMPLETED;

                business.RegisteredTime = DateTime.Now;
                business.ExpiredTime = DateTime.Now.AddMonths(pack.Period);
                business.Status = StatusConstants.ACTIVE;

                await _subscriptionPackRepository.RunInTransactionAsync(async () =>
                {
                    await _transactionRepository.UpdateTransactionAsync(transaction);

                    await _businessRepository.UpdateBusinessProfileAsync(business);
                });

                return new ApiResponse<PaymentResponse>("success", "Thanh Toán Gói Đăng Kí Thành Công!", null, 200);
            }
            else
            {
                if (result.VnPayResponseCode.Equals("11"))
                {
                    transaction.Status = StatusConstants.EXPIRED;
                }
                else
                {
                    transaction.Status = StatusConstants.FAILED;
                }

                await _transactionRepository.UpdateTransactionAsync(transaction);

                return new ApiResponse<PaymentResponse>("error", 400, "Thanh Toán Gói Đăng Kí Thất Bại!");
            }
        }

        public Task<ApiResponse<PaymentResponse>> ZaloPayExcuteAsync(IQueryCollection collection)
        {
            throw new NotImplementedException();
        }
    }
}
