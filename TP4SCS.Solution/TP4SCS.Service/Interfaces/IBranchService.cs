using TP4SCS.Library.Models.Request.Branch;
using TP4SCS.Library.Models.Response.Branch;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IBranchService
    {
        Task<ApiResponse<IEnumerable<BranchResponse>?>> GetBranchesByBusinessIdAsync(int id);

        Task<ApiResponse<BranchResponse?>> GetBranchByIdAsync(int id);

        Task<ApiResponse<BranchResponse?>> CreateBranchAsync(CreateBranchRequest createBranchRequest);

        Task<ApiResponse<BranchResponse?>> UpdateBranchAsync(int id, UpdateBranchRequest updateBranchRequest);
        
        Task<ApiResponse<BranchResponse?>> UpdateBranchEmployeeAsync(string employeeIds);

        Task<ApiResponse<BranchResponse?>> UpdateBranchStatusAsync(string status);
    }
}