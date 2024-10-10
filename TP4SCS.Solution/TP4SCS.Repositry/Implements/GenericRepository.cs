using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class GenericRepoistory<T> : IGenericRepository<T> where T : class
    {
        protected readonly Tp4scsDevDatabaseContext _dbContext;
        protected DbSet<T> dbSet;

        public GenericRepoistory(Tp4scsDevDatabaseContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> Get(
           Expression<Func<T, bool>>? filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
           string includeProperties = "",
           int? pageIndex = null, // Optional parameter for pagination (page number)
           int? pageSize = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return await query.ToListAsync(); // Sử dụng ToListAsync để thực hiện truy vấn không đồng bộ
        }

        public virtual async Task<T> GetByID(object id)
        {
            return await dbSet.FindAsync(id); // Sử dụng FindAsync để tìm kiếm không đồng bộ
        }

        public virtual async Task Insert(T entity)
        {
            await dbSet.AddAsync(entity); // Sử dụng AddAsync để thêm không đồng bộ
            await _dbContext.SaveChangesAsync(); // Lưu thay đổi không đồng bộ
        }

        public virtual async Task Delete(object id)
        {
            T entityToDelete = await dbSet.FindAsync(id); // Tìm kiếm không đồng bộ
            await Delete(entityToDelete); // Gọi phương thức xóa không đồng bộ
        }

        public virtual async Task Delete(T entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            await _dbContext.SaveChangesAsync(); // Lưu thay đổi không đồng bộ
        }

        public virtual async Task Update(T entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(); // Lưu thay đổi không đồng bộ
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
