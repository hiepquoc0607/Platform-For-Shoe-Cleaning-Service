using System.ComponentModel.DataAnnotations;
using TP4SCS.Library.Models.Request.Branch;

namespace TP4SCS.Library.Models.Request.BusinessProfile
{
    public class CreateBusinessRequest
    {
        [Required]
        public CreateBusinessObject CreateBusiness { get; set; } = new CreateBusinessObject();

        [Required]
        public CreateBranchRequest CreateBranch { get; set; } = new CreateBranchRequest();
    }
}
