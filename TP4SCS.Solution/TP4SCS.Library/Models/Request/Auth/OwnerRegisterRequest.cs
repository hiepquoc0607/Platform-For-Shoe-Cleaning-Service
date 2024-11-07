using System.ComponentModel.DataAnnotations;
using TP4SCS.Library.Models.Request.Branch;
using TP4SCS.Library.Models.Request.BusinessProfile;

namespace TP4SCS.Library.Models.Request.Auth
{
    public class OwnerRegisterRequest
    {
        [Required]
        public CustomerRegisterRequest CustomerRegister { get; set; } = new CustomerRegisterRequest();

        [Required]
        public CreateBusinessRequest CreateBusiness { get; set; } = new CreateBusinessRequest();

        [Required]
        public CreateBranchRequest CreateBranch { get; set; } = new CreateBranchRequest();
    }
}
