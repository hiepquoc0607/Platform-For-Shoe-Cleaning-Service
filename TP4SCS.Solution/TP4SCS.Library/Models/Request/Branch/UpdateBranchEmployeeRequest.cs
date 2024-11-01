using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Branch
{
    public class UpdateBranchEmployeeRequest
    {
        [Required]
        public string EmployeeIds { get; set; } = string.Empty;

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
