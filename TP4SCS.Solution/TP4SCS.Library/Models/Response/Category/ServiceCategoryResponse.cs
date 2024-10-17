using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4SCS.Library.Models.Response.Category
{
    public class ServiceCategoryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public required string Status { get; set; }
    }
}
