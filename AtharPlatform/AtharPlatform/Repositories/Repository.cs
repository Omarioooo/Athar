using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace AtharPlatform.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly Context _context;
        protected DbSet<T> _dbSet { get; set; }

        public Repository(Context context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public virtual async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual async Task<T?> GetAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Database error : Entity with id {id} of type {typeof(T).Name} not found.");
            return entity;
        }

        public virtual async Task<T> GetWithExpressionAsync(Expression<Func<T, bool>> expression)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(expression);

            if (entity == null)
                throw new KeyNotFoundException($"Database error : Entity of type {typeof(T).Name} not found.");

            return entity;
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException($"Database error : {nameof(entity)}");

            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException($"Database error : {nameof(entity)}");

            _dbSet.Update(entity);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Database error : Cannot delete. Entity with id {id} not found.");

            _dbSet.Remove(entity);
            return true;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

    }
}
