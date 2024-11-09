using Mapster;
using MapsterMapper;
using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Library.Models.Response.BusinessProfile;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IBusinessBranchService _branchService;
        private readonly IMapper _mapper;
        private readonly BusinessUtil _util;

        public BusinessService(IBusinessRepository businessRepository, IBusinessBranchService branchService, IMapper mapper, BusinessUtil util)
        {
            _businessRepository = businessRepository;
            _branchService = branchService;
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

        //Create Business
        //public async Task<ApiResponse<BusinessResponse>> CreateBusinessProfileAsync(int id, OwnerRegisterRequest createBusinessRequest)
        //{
        //    var businessData = createBusinessRequest.CreateBusiness;
        //    var branchData = createBusinessRequest.CreateBranch;

        //    //var isPhoneExisted = await _businessRepository.IsPhoneExistedAsync(businessData.Phone.Trim());

        //    //if (isPhoneExisted)
        //    //{
        //    //    return new ApiResponse<BusinessResponse>("error", 400, "Số Điện Thoại Doanh Nghiệp Đã Được Sử Dụng!");
        //    //}

        //    var isNameExisted = await _businessRepository.IsNameExistedAsync(businessData.Name.Trim().ToLower());

        //    if (isNameExisted)
        //    {
        //        return new ApiResponse<BusinessResponse>("error", 400, "Tên Doanh Nghiệp Đã Được Sử Dụng!");
        //    }

        //    var newBusiness = _mapper.Map<BusinessProfile>(businessData);
        //    newBusiness.OwnerId = id;

        //    var newBranch = _mapper.Map<BusinessBranch>(branchData);
        //    newBranch.BusinessId = await _businessRepository.GetBusinessProfileMaxIdAsync() + 1;

        //    try
        //    {
        //        await _businessRepository.CreateBusinessProfileAsync(newBusiness, newBranch);

        //        var newId = await _businessRepository.GetBusinessProfileMaxIdAsync();

        //        var newBsn = await GetBusinessProfileByIdAsync(newId);

        //        return new ApiResponse<BusinessResponse>("success", "Tạo Doanh Nghiệp Thành Công!", newBsn.Data);
        //    }
        //    catch (Exception)
        //    {
        //        return new ApiResponse<BusinessResponse>("error", 400, "Tạo Doanh Nghiệp Thất Bại!");
        //    }
        //}

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

        //Update Business Rank
        public async Task<ApiResponse<BusinessResponse>> UpdateBusinessRankAsync(int id, UpdateBusinessRankRequest updateBusinessRankRequest)
        {
            var oldBusiness = await _businessRepository.GetBusinessProfileByIdAsync(id);

            if (oldBusiness == null)
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Tài Khoản Chưa Sở Hữu Doanh Nghiệp!");
            }

            oldBusiness.Rank = updateBusinessRankRequest.Rank;

            try
            {
                await _businessRepository.UpdateBusinessProfileAsync(oldBusiness);

                return new ApiResponse<BusinessResponse>("success", "Cập Nhập Xếp Hạng Doanh Nghiệp Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<BusinessResponse>("error", 400, "Cập Nhập Xếp Hạng Doanh Nghiệp Thất Bại!");
            }
        }

        public Task<ApiResponse<BusinessResponse>> UpdateBusinessStatisticAsync(int id, UpdateBusinessStatisticRequest updateBusinessStatisticRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<BusinessResponse>> UpdateBusinessStatusForAdminAsync(int id, UpdateBusinessStatusRequest updateBusinessStatusRequest)
        {
            if (!_util.CheckStatusForAdmin(updateBusinessStatusRequest.Status))
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Trạng Thái Không Khả Dụng!");
            }

            var oldBusiness = await _businessRepository.GetBusinessByOwnerIdAsync(id);

            if (oldBusiness == null)
            {
                return new ApiResponse<BusinessResponse>("error", 404, "Tài Khoản Chưa Sở Hữu Doanh Nghiệp!");
            }

            oldBusiness.Status = updateBusinessStatusRequest.Status;

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

        public async Task<int?> GetBusinessIdByOwnerId(int id)
        {
            return await _businessRepository.GetBusinessIdByOwnerIdAsync(id);
        }
    }
}
