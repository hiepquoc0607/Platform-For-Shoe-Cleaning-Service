using MapsterMapper;
using TP4SCS.Library.Models.Request.Branch;
using TP4SCS.Library.Models.Response.Branch;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public BranchService(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public Task<ApiResponse<BranchResponse?>> CreateBranchAsync(CreateBranchRequest createBranchRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<BranchResponse?>> GetBranchByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IEnumerable<BranchResponse>?>> GetBranchesByBusinessIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<BranchResponse?>> UpdateBranchAsync(int id, UpdateBranchRequest updateBranchRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<BranchResponse?>> UpdateBranchEmployeeAsync(string employeeIds)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<BranchResponse?>> UpdateBranchStatusAsync(string status)
        {
            throw new NotImplementedException();
        }
    }
}
