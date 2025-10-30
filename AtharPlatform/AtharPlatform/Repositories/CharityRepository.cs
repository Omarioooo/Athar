using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace AtharPlatform.Repositories
{
    public class CharityRepository : Repository<Charity>, ICharityRepository
    {
        public CharityRepository(Context context) : base(context) { }

        public override Task<List<Charity>> GetAllAsync() => base.GetAllAsync();

        public override Task<Charity?> GetAsync(int id) => base.GetAsync(id);

        public override Task<Charity?> GetAsync(Expression<Func<Charity, bool>> expression) => base.GetAsync(expression);

        public override Task<bool> AddAsync(Charity entity) => base.AddAsync(entity);

        public override Task<bool> Update(Charity entity) => base.Update(entity);

        public override Task<bool> DeleteAsync(int id) => base.DeleteAsync(id);

        public async Task<List<Charity>> GetPageAsync(string? query, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 12;

            var q = _dbSet.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(c => c.Name.Contains(term));
            }
            return await q
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.campaigns)
                .Include(c => c.ScrapedInfo)
                .ToListAsync();
        }

        public async Task<int> CountAsync(string? query)
        {
            var q = _dbSet.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(c => c.Name.Contains(term));
            }
            return await q.CountAsync();
        }

        public async Task<Charity?> GetWithCampaignsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.campaigns)
                .Include(c => c.ScrapedInfo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task BulkImportAsync(IEnumerable<Charity> charities)
        {
            if (charities == null) return;
            await _dbSet.AddRangeAsync(charities);
        }
    }
}
