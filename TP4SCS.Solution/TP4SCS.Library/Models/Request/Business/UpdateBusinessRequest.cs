using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Business
{
    public class UpdateBusinessRequest
    {
        [DefaultValue("string")]
        public string Name { get; set; } = string.Empty;

        [Phone]
        [StringLength(10, MinimumLength = 10)]
        [DefaultValue("string")]
        public string Phone { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string ImageUrl { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string Status { get; set; } = string.Empty;
    }
}
