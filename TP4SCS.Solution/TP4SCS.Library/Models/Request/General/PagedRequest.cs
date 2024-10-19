using System.Runtime.Serialization;

namespace TP4SCS.Library.Models.Request.General
{
    public class PagedRequest
    {
        public string? Keyword { get; set; } = null;
        public string? Status { get; set; } = null;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public OrderByEnum OrderBy { get; set; } = OrderByEnum.IdAsc;
    }

    public enum OrderByEnum
    {
        [EnumMember(Value = "Id Ascending")]
        IdAsc,

        [EnumMember(Value = "Id Descending")]
        IdDesc
    }
}
