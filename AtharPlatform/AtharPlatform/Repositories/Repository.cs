using System.Linq.Expressions;

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

        public virtual async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual async Task<T?> GetAsync(int id) => await _dbSet.FindAsync(id);

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(expression);

            if (entity == null)
                throw new KeyNotFoundException($"Entity of type {typeof(T).Name} not found.");

            return entity;
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);

            return true;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            return true;
        }

    }
}
