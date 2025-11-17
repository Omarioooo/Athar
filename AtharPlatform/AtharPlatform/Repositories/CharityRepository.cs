using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace AtharPlatform.Repositories
{
    public class CharityRepository : Repository<Charity>, ICharityRepository
    {
        public CharityRepository(Context context) : base(context) { }

        public async override Task<Charity?> GetAsync(int id)
        {
            var charity = await _dbSet
                .Include(c => c.Account)
                .Where(c => c.IsActive)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (charity == null)
                throw new KeyNotFoundException($"Charity with id {id} not found");

            return charity;
        }

        public async override Task<List<Charity>> GetAllAsync()
        {
            var charities = await _dbSet
                .Include(c => c.Account)
                .Where(c => c.IsActive)
                .ToListAsync();

            if (charities == null)
                throw new KeyNotFoundException($"Charities not found");

            return charities;
        }

        public async override Task<Charity> GetWithExpressionAsync(Expression<Func<Charity, bool>> expression)
        {
            var Charitys = await _dbSet
                .Include(c => c.Account)
                .Where(c => c.IsActive)
                .FirstOrDefaultAsync(expression);

            if (Charitys == null)
                throw new KeyNotFoundException($"Charity not found");

            return Charitys;
        }



        public async Task<Charity> GetCharityByCampaignAsync(int campaignId)
        {
            var charity = await _dbSet
                .Include(c => c.Account)
                .Include(c => c.Campaigns)
                .FirstOrDefaultAsync(c => c.Campaigns.Any(cm => cm.Id == campaignId));

            return charity;
        }

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
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.Campaigns)
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
            return await q.Where(c => c.IsActive).CountAsync();
        }

        public async Task<Charity?> GetWithCampaignsAsync(int id)
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .Include(c => c.Account)
                .Include(c => c.Campaigns)
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
