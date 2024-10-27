using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task AddMaterialAsync(int serviceId, Material material)
        {

            if (material == null)
            {
                throw new ArgumentNullException(nameof(material), "Material không được null.");
            }
            if (string.IsNullOrWhiteSpace(material.Name))
            {
                throw new ArgumentException("Name không được bỏ trống.", nameof(material.Name));
            }

            if (material.Price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(material.Price), "Price phải lớn hơn 0.");
            }

            if (material.Storage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(material.Storage), "Storage không thể âm.");
            }
            await _materialRepository.AddMaterialAsync(serviceId,material);
        }

        public async Task DeleteMaterialAsync(int id)
        {
            var material = await _materialRepository.GetMaterialByIdAsync(id);
            if(material == null)
            {
                throw new Exception($"Vật liệu với ID {id} không tìm thấy.");
            }
            await _materialRepository.DeleteMaterialAsync(id);
        }

        public async Task<Material?> GetMaterialByIdAsync(int id)
        {
            return await _materialRepository.GetMaterialByIdAsync(id);
        }

        public async Task<IEnumerable<Material>?> GetMaterialsAsync(
            string? keyword = null,
            string? status = null,
            int pageIndex = 1,
            int pageSize = 5,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            return await _materialRepository.GetMaterialsAsync(keyword, status, pageIndex, pageSize, orderBy);
        }

        public async Task<IEnumerable<Material>> GetAllMaterialsAsync(string? keyword = null, string? status = null)
        {
            return await _materialRepository.GetMaterialsAsync(keyword, status);
        }

        public async Task<int> GetTotalMaterialCountAsync(string? keyword = null, string? status = null)
        {
            return await _materialRepository.GetTotalMaterialCountAsync(keyword, status);
        }

        public async Task UpdateMaterialAsync(int id, Material material)
        {
            if (material == null)
            {
                throw new ArgumentNullException(nameof(material), "Material không được null.");
            }

            if (string.IsNullOrWhiteSpace(material.Name))
            {
                throw new ArgumentException("Name không được bỏ trống.", nameof(material.Name));
            }

            if (material.Price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(material.Price), "Price phải lớn hơn 0.");
            }

            if (material.Storage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(material.Storage), "Storage không thể âm.");
            }

            if (string.IsNullOrWhiteSpace(material.Status))
            {
                throw new ArgumentException("Status không được bỏ trống.", nameof(material.Status));
            }

            var existingMaterial = await _materialRepository.GetMaterialByIdAsync(id);
            if (existingMaterial == null)
            {
                throw new KeyNotFoundException($"Material với ID {id} không tìm thấy.");
            }

            existingMaterial.Name = material.Name;
            existingMaterial.Price = material.Price;
            existingMaterial.Storage = material.Storage;
            existingMaterial.Status = material.Status;

            await _materialRepository.UpdateMaterialAsync(existingMaterial);
        }

    }
}
