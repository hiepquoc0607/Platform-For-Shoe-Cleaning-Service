using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.General
{
    public class GetAccountRequest
    {
        public string? SearchKey { get; set; } = string.Empty;

        public string? SortBy { get; set; } = string.Empty;

        [DefaultValue(false)]
        public bool IsDecsending { get; set; }

        [DefaultValue(10)]
        public int PageSize { get; set; }

        [DefaultValue(1)]
        public int PageNum { get; set; }
    }
}
