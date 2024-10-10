using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request;

namespace TP4SCS.Services.Interfaces
{
    public interface IServiceCategoryService
    {
        Task<ServiceCategory?> GetServiceCategoryById(int id);
        Task<IEnumerable<ServiceCategory>> GetServiceCategories(string? keyword = null, int pageIndex = 1, int pageSize = 5, string orderBy = "Name");
        Task AddServiceCategory(ServiceCategory category);
        Task UpdateServiceCategory(ServiceCategory category, int existingCategoryId);
        Task DeleteServiceCategory(int id);
    }
}
