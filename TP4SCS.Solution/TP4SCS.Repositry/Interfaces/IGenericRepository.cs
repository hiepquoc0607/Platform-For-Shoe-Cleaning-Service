using System.Linq.Expressions;

namespace TP4SCS.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get(
           Expression<Func<T, bool>>? filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
           string includeProperties = "",
           int? pageIndex = null,
           int? pageSize = null);
        Task<T> GetByID(object id);
        Task Insert(T entity);
        Task Delete(object id);
        Task Delete(T entityToDelete);
        Task Update(T entityToUpdate);
        Task SaveAsync();
    }
}

