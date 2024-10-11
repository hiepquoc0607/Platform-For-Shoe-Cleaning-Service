using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Account
{
    public enum RoleOption
    {
        Customer = 0,
        Owner = 1,
        Employee = 2,
        Moderator = 3
    }

    public class RoleRequest
    {
        [Required]
        public RoleOption CreateRole { get; set; } = RoleOption.Customer;
    }
}
