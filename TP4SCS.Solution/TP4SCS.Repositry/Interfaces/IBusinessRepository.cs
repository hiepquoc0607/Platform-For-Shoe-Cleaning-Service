using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Repository.Interfaces
{
    public interface IBusinessRepository : IGenericRepository<BusinessProfile>
    {
        Task<int?> GetBusinessIdByOwnerIdAsync(int id);

        Task<BusinessProfile?> GetBusinessByOwnerIdAsync(int id);

        Task<(IEnumerable<BusinessProfile>?, Pagination)> GetBusinessesProfilesAsync(GetBusinessRequest getBusinessRequest);

        Task<BusinessProfile?> GetBusinessProfileByIdAsync(int id);

        Task<int> GetBusinessProfileMaxIdAsync();

        Task<int> CountBusinessServiceByIdAsync(int id);

        Task<bool> IsNameExistedAsync(string name);

        Task<bool> IsPhoneExistedAsync(string phone);

        Task CreateBusinessProfileAsync(BusinessProfile businessProfile, BusinessBranch businessBranch);

        Task UpdateBusinessProfileAsync(BusinessProfile businessProfile);
    }
}
