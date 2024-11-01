using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Business
{
    public class GetBusinessRequest
    {
        public string? SearchKey { get; set; } = string.Empty;

        public string? SortBy { get; set; } = string.Empty;

        public string? Status { get; set; } = string.Empty;

        [Required]
        [DefaultValue(false)]
        public bool IsDecsending { get; set; }

        [Required]
        [DefaultValue(10)]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }

        [Required]
        [DefaultValue(1)]
        [Range(1, int.MaxValue)]
        public int PageNum { get; set; }
    }
}
