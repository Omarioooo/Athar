using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    // Repo for base CRUDs
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<bool> AddAsync(T entity);
        Task<bool> Update(T entity);
        Task<bool> DeleteAsync(int id);
    }
}

