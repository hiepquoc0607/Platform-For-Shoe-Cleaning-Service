using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class ServiceCategoryService : IServiceCategoryService
    {
        private IServiceCategoryRepository _categoryRepository;

        public ServiceCategoryService(IServiceCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task AddServiceCategoryAsync(ServiceCategory category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Category name is required.");
            }
            if (category.Name.Length < 3 || category.Name.Length > 100)
            {
                throw new ArgumentException("Category name must be between 3 and 100 characters.");
            }
            if (!string.IsNullOrEmpty(category.Description) && category.Description.Length > 500)
            {
                throw new ArgumentException("Description cannot below exceed 500 characters.");
            }
            if (string.IsNullOrEmpty(category.Description) || category.Description.Length < 10)
            {
                throw new ArgumentException("Description must be at least 10 characters long.");
            }
            await _categoryRepository.AddCategory(category);
        }

        public async Task DeleteServiceCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                throw new Exception($"Category with ID {id} not found.");
            }
            await _categoryRepository.DeleteCategory(id);
        }

        public async Task<IEnumerable<ServiceCategory>?> GetServiceCategoriesAsync(string? keyword = null, int pageIndex = 1, int pageSize = 5, string orderBy = "Name")
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("Page index must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Page size must be greater than 0.");
            }
            return await _categoryRepository.GetCategories(keyword, pageIndex, pageSize, orderBy);
        }

        public async Task<ServiceCategory?> GetServiceCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetCategoryById(id);
        }

        public async Task UpdateServiceCategoryAsync(ServiceCategory category, int existingCategoryId)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Category name is required.");
            }
            if (category.Name.Length < 3 || category.Name.Length > 100)
            {
                throw new ArgumentException("Category name must be between 3 and 100 characters.");
            }
            if (!string.IsNullOrEmpty(category.Description) && category.Description.Length > 500)
            {
                throw new ArgumentException("Description cannot exceed 500 characters.");
            }
            if (string.IsNullOrEmpty(category.Description) || category.Description.Length < 10)
            {
                throw new ArgumentException("Description must be at least 10 characters long.");
            }
            var existingCategory = await _categoryRepository.GetCategoryById(existingCategoryId);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Service with ID {existingCategoryId} not found.");
            }
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            await _categoryRepository.UpdateCategory(existingCategory);
        }
    }
}
