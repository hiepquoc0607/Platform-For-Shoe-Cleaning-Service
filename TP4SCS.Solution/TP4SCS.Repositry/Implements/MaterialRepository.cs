using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task AddMaterialAsync(int serviceId, Material material)
        {
            Service? service = null;

            service = await _dbContext.Services.SingleOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                throw new KeyNotFoundException($"Service với ID {serviceId} không tìm thấy.");
            }
            if(service.Status.ToLower() == StatusConstants.Inactive.ToLower())
            {
                throw new ArgumentException("Dịch vụ này đã ngưng hoạt động.");
            }
            var serviceMaterial = new ServiceMaterial { Service = service, Material = material };

            await _dbContext.ServiceMaterials.AddAsync(serviceMaterial);
            await _dbContext.Materials.AddAsync(material);
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteMaterialAsync(int id)
        {
            var serviceMaterials = await _dbContext.ServiceMaterials
                                                 .Where(sm => sm.MaterialId == id)
                                                 .ToListAsync();

            if (serviceMaterials.Any())
            {
                _dbContext.ServiceMaterials.RemoveRange(serviceMaterials);
                await _dbContext.SaveChangesAsync();
            }
            await DeleteAsync(id);
        }


        public async Task<Material?> GetMaterialByIdAsync(int id)
        {
            return await GetByIDAsync(id);
        }

        public Task<IEnumerable<Material>?> GetMaterialsAsync(
            string? keyword = null,
            string? status = null,
            int pageIndex = 1,
            int pageSize = 5,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            Expression<Func<Material, bool>> filter = m =>
                (string.IsNullOrEmpty(keyword) || m.Name.ToLower().Trim().Contains(keyword.ToLower().Trim())) &&
                (string.IsNullOrEmpty(status) || m.Status.ToLower().Trim() == status.ToLower().Trim());

            Func<IQueryable<Material>, IOrderedQueryable<Material>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(c => c.Id),
                _ => q.OrderBy(c => c.Id)
            };

            return GetAsync(
                filter: filter,
                orderBy: orderByExpression,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }

        public async Task<IEnumerable<Material>> GetMaterialsAsync(string? keyword = null, string? status = null)
        {
            Expression<Func<Material, bool>> filter = m =>
                (string.IsNullOrEmpty(keyword) || m.Name.Contains(keyword)) &&
                (string.IsNullOrEmpty(status) || m.Status.ToLower() == status.ToLower());

            return await _dbContext.Materials.AsNoTracking()
                .Where(filter)
                .ToListAsync();
        }

        public async Task<int> GetTotalMaterialCountAsync(string? keyword = null, string? status = null)
        {
            Expression<Func<Material, bool>> filter = m =>
                (string.IsNullOrEmpty(keyword) || m.Name.Contains(keyword)) &&
                (string.IsNullOrEmpty(status) || m.Status.ToLower() == status.ToLower());

            return await _dbContext.Materials.AsNoTracking().CountAsync(filter);
        }

        public async Task UpdateMaterialAsync(Material material)
        {
            await UpdateAsync(material);
        }
    }
}
