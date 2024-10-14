using TP4SCS.Library.Models.Data;

namespace TP4SCS.Services.Interfaces
{
    public interface IServiceCategoryService
    {
        Task<ServiceCategory?> GetServiceCategoryByIdAsync(int id);
        Task<IEnumerable<ServiceCategory>?> GetServiceCategoriesAsync(string? keyword = null, int pageIndex = 1, int pageSize = 5, string orderBy = "Name");
        Task AddServiceCategoryAsync(ServiceCategory category);
        Task UpdateServiceCategoryAsync(ServiceCategory category, int existingCategoryId);
        Task DeleteServiceCategoryAsync(int id);
    }
}
