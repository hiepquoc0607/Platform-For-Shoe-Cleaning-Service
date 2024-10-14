using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4SCS.Library.Models.Request.General
{
    public class PagedRequest
    {
        public string? Keyword { get; set; } = null;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string OrderBy { get; set; } = "Id";
    }
}
