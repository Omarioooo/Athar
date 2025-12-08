using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    // Repo for base CRUDs
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> GetWithExpressionAsync(Expression<Func<T, bool>> expression);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll();
     

    }
}