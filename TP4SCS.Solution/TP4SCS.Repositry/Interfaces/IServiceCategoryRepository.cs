using TP4SCS.Library.Models.Data;

namespace TP4SCS.Repository.Interfaces
{
    public interface IServiceCategoryRepository
    {
        Task<IEnumerable<ServiceCategory>?> GetCategories(string? keyword = null,
        int pageIndex = 1,
        int pageSize = 5,
        string orderBy = "Id");
        Task<ServiceCategory?> GetCategoryById(int id);
        Task AddCategory(ServiceCategory category);
        Task UpdateCategory(ServiceCategory category);
        Task DeleteCategory(int id);
    }
}
