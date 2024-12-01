using Mapster;
using MapsterMapper;
using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Library.Models.Response.BusinessProfile;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IBusinessBranchService _branchService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly Util _util;

        public BusinessService(IBusinessRepository businessRepository,
            IAccountRepository accountRepository,
            IBusinessBranchService branchService,
            IEmailService emailService,
            IMapper mapper,
            Util util)
        {
            _businessRepository = businessRepository;
            _accountRepository = accountRepository;
            _branchService = branchService;
            _emailService = emailService;
            _mapper = mapper;
            _util = util;
        }

        //Check Business Owner
        public async Task<bool> CheckOwnerOfBusiness(int ownerId, int businessId)
        {
            var id = await _businessRepository.GetBusinessIdByOwnerIdAsync(ownerId);

            if (id != businessId)
            {
                return false;
            }

            return true;
        }

        //Get Businesses
        public async Task<ApiResponse<IEnumerable<BusinessResponse>?>> GetBusinessesProfilesAsync(GetBusinessRequest getBusinessRequest)
        {
            var (businesses, pagination) = await _businessRepository.GetBusinessesProfilesAsync(getBusinessRequest);

            if (businesses == null)
            {
                return new ApiResponse<IEnumerable<BusinessResponse>?>("error", 404, "Không Tìm Thấy Doanh Nghiệp!");
            }

            var data = businesses.Adapt<IEnumerable<BusinessResponse>>();

            return new ApiResponse<IEnumerable<BusinessResponse>?>("success", "Lấy Dữ Liệu Thành Công!", data, 200, pagination);
        }

        //Get Business By Id
        public async Task<ApiResponse<BusinessResponse?>> GetBusinessProfileByIdAsync(int id)
        {
            var business = await _businessRepository.GetBusinessProfileByIdAsync(id);

            if (business == null)
            {
                return new ApiResponse<BusinessResponse?>("error", 404, "Không Tìm Thấy Doanh Nghiệp!");
            }

            var data = _mapper.Map<BusinessResponse>(business);

            return new ApiResponse<BusinessResponse?>("success", "Lấy Dữ Liệu Thành Công!", data);
        }

        //Update Business
        public async Task<ApiResponse<BusinessResponse>> UpdateBusinessProfileAsync(int id, UpdateBusinessRequest updateBusinessRequest)
        {
            if (!_util.CheckStatus(updateBusinessRequest.Status))
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Trạng Thái Không Khả Dụng!");
            }

            var oldBusiness = await _businessRepository.GetBusinessProfileByIdAsync(id);

            if (oldBusiness == null)
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Tài Khoản Chưa Sở Hữu Doanh Nghiệp!");
            }

            var newBusiness = _mapper.Map(updateBusinessRequest, oldBusiness);
            newBusiness.Name = _util.FormatStringName(updateBusinessRequest.Name);
            newBusiness.Status = oldBusiness.Status switch
            {
                StatusConstants.UNREGISTERED => StatusConstants.UNREGISTERED,
                StatusConstants.EXPIRED => StatusConstants.EXPIRED,
                StatusConstants.SUSPENDED => StatusConstants.SUSPENDED,
                _ => updateBusinessRequest.Status,
            };

            try
            {
                await _businessRepository.UpdateBusinessProfileAsync(newBusiness);

                return new ApiResponse<BusinessResponse>("success", "Cập Nhập Doanh Nghiệp Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<BusinessResponse>("error", 400, "Cập Nhập Doanh Nghiệp Thất Bại!");
            }
        }

        public async Task<ApiResponse<BusinessResponse>> UpdateBusinessStatisticAsync(int id, UpdateBusinessStatisticRequest updateBusinessStatisticRequest)
        {
            var business = await _businessRepository.GetBusinessProfileByIdAsync(id);

            if (business == null)
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Không Tìm Thấy Thông Tin Doanh Nghiệp!");
            }

            switch (updateBusinessStatisticRequest.Type)
            {
                case OrderStatistic.PENDING:
                    business.PendingAmount += 1;
                    break;
                case OrderStatistic.PROCESSING:
                    business.PendingAmount = (business.PendingAmount - 1 < 0) ? 0 : business.PendingAmount - 1;
                    business.ProcessingAmount += 1;
                    break;
                case OrderStatistic.FINISHED:
                    business.ProcessingAmount = (business.ProcessingAmount - 1 < 0) ? 0 : business.ProcessingAmount - 1;
                    business.FinishedAmount += 1;
                    business.TotalOrder += 1;
                    break;
                case OrderStatistic.CANCELED:
                    business.PendingAmount = (business.PendingAmount - 1 < 0) ? 0 : business.PendingAmount - 1;
                    business.CanceledAmount += 1;
                    break;
                default:
                    break;
            }

            try
            {
                await _businessRepository.UpdateBusinessProfileAsync(business);

                return new ApiResponse<BusinessResponse>("success", "Cập Nhập Thống Kê Doanh Nghiệp Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<BusinessResponse>("error", 400, "Cập Nhập Thống Kê Doanh Nghiệp Thất Bại!");
            }
        }

        public async Task<ApiResponse<BusinessResponse>> UpdateBusinessStatusForAdminAsync(int id, UpdateBusinessStatusRequest updateBusinessStatusRequest)
        {
            var oldBusiness = await _businessRepository.GetBusinessProfileByIdAsync(id);

            if (oldBusiness == null)
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Không Tìm Thấy Thông Tin Doanh Nghiệp!");
            }

            oldBusiness.Status = updateBusinessStatusRequest.Status switch
            {
                BusinessStatus.INACTIVE => StatusConstants.INACTIVE,
                BusinessStatus.SUSPENDED => StatusConstants.SUSPENDED,
                BusinessStatus.EXPRIED => StatusConstants.EXPIRED,
                _ => StatusConstants.ACTIVE,
            };

            try
            {
                await _businessRepository.UpdateBusinessProfileAsync(oldBusiness);

                return new ApiResponse<BusinessResponse>("success", "Cập Nhập Trạng Thái Doanh Nghiệp Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<BusinessResponse>("error", 400, "Cập Nhập Trạng Thái Doanh Nghiệp Thất Bại!");
            }
        }

        public async Task<ApiResponse<BusinessResponse>> UpdateBusinessSubscriptionAsync(int id, UpdateBusinessSubcriptionRequest updateBusinessSubcriptionRequest)
        {
            if (!_util.CheckDateTime(updateBusinessSubcriptionRequest.RegisteredTime, updateBusinessSubcriptionRequest.ExpiredTime))
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Trường Nhập Không Khả Dụng!");
            }

            var oldBusiness = await _businessRepository.GetBusinessByOwnerIdAsync(id);

            if (oldBusiness == null)
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Tài Khoản Chưa Sở Hữu Doanh Nghiệp!");
            }

            var newBusniess = _mapper.Map(updateBusinessSubcriptionRequest, oldBusiness);

            try
            {
                await _businessRepository.UpdateBusinessProfileAsync(newBusniess);

                return new ApiResponse<BusinessResponse>("success", "Cập Nhập Gói Doanh Nghiệp Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<BusinessResponse>("error", 400, "Cập Nhập Gói Doanh Nghiệp Thất Bại!");
            }
        }

        public async Task<int?> GetBusinessIdByOwnerIdAsync(int id)
        {
            return await _businessRepository.GetBusinessIdByOwnerIdAsync(id);
        }

        public async Task<ApiResponse<IEnumerable<BusinessResponse>?>> GetInvalidateBusinessesProfilesAsync(GetInvalidateBusinessRequest getInvalidateBusinessRequest)
        {
            var (businesses, pagination) = await _businessRepository.GetInvlaidateBusinessesProfilesAsync(getInvalidateBusinessRequest);

            if (businesses == null)
            {
                return new ApiResponse<IEnumerable<BusinessResponse>?>("error", 404, "Không Tìm Thấy Doanh Nghiệp!");
            }

            var data = businesses.Adapt<IEnumerable<BusinessResponse>>();

            return new ApiResponse<IEnumerable<BusinessResponse>?>("success", "Lấy Dữ Liệu Thành Công!", data, 200, pagination);
        }

        public async Task<ApiResponse<BusinessResponse>> ValidateBusinessAsync(int id, ValidateBusinessRequest validateBusinessRequest)
        {
            var oldBusiness = await _businessRepository.GetBusinessProfileByIdAsync(id);

            if (oldBusiness == null)
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Không Tìm Thấy Doanh Nghiệp!");
            }

            if (!oldBusiness.Status.Equals(StatusConstants.PENDING))
            {
                return new ApiResponse<BusinessResponse>("error", 400, "Doanh Nghiệp Đã Được Xác Nhận Trước Đó!");
            }

            string email = await _accountRepository.GetAccountEmailByIdAsync(oldBusiness.OwnerId); ;
            string emailBody = "";

            try
            {
                if (validateBusinessRequest.IsApprove)
                {
                    oldBusiness.Status = StatusConstants.EXPIRED;

                    await _businessRepository.RunInTransactionAsync(async () =>
                    {
                        await _businessRepository.UpdateAsync(oldBusiness);

                        emailBody = "Xác Nhận Thông Tin Doanh Nghiệp Hợp Lệ, Bạn Có Thể Mua Gói Dịch Vụ Và Bắt Đầu Sử Dụng!";
                    });
                }
                else
                {
                    await _businessRepository.RunInTransactionAsync(async () =>
                    {
                        await _businessRepository.DeleteAsync(oldBusiness);

                        emailBody = "Xác Nhận Thông Tin Doanh Nghiệp Bị Từ Chối, Vui Lòng Kiểm Tra Lại CCCD Và Ảnh Cung Cấp!";
                    });
                }

                _ = _emailService.SendEmailAsync(email, "Shoe Care Hub Xác Nhận Doanh Nghiệp", emailBody);

                return new ApiResponse<BusinessResponse>("success", "Xác Nhận Doanh Nghiệp Thành Công!", null, 200);
            }
            catch (Exception)
            {
                return new ApiResponse<BusinessResponse>("error", 400, "Xác Nhận Doanh Nghiệp Thất Bại!");
            }

        }
    }
}
