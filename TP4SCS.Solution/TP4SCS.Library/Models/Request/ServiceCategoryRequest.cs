using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4SCS.Library.Models.Request
{
    public class ServiceCategoryRequest
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
