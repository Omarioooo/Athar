

using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class CharityRepository : Repository<Charity>, ICharityRepository
    {
        public CharityRepository(Context context) : base(context) { }

        public async override Task<Charity?> GetAsync(int id)
        {
            var charity = await _dbSet.Include(c => c.Account)
                 .FirstOrDefaultAsync(c => c.Id == id);

            if (charity == null)
                throw new KeyNotFoundException($"Charity with id {id} not found");

            return charity;
        }

        public async override Task<List<Charity>> GetAllAsync()
        {
            var charities = await _dbSet.Include(c => c.Account)
                .ToListAsync();

            if (charities == null)
                throw new KeyNotFoundException($"Charities not found");

            return charities;
        }

        public async override Task<Charity> GetWithExpressionAsync(Expression<Func<Charity, bool>> expression)
        {
            var Charitys = await _dbSet.Include(c => c.Account)
                .FirstOrDefaultAsync(expression);

            if (Charitys == null)
                throw new KeyNotFoundException($"Charity not found");

            return Charitys;
        }

        public async Task<List<int>> GetCharitySubscribersAsync(int id)
        {
            var donorIds = await _context.Subscriptions
                .Where(f => f.CharityId == id)
                .Select(f => f.DonorId)
                .ToListAsync();

            return donorIds;
        }

        public async Task<Charity> GetCharityByCampaignAsync(int campaignId)
        {
            var charity = await _context.Charities
                .Include(c => c.Campaigns)
                .FirstOrDefaultAsync(c => c.Campaigns.Any(cm => cm.Id == campaignId));

            return charity;
        }

        public async Task<int> CountAsync(string? query)
        {
            var q = _context.Charities.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(c => c.Name.Contains(term) || c.Description.Contains(term));
            }
            return await q.CountAsync();
        }

        public async Task<List<Charity>> GetPageAsync(string? query, int page, int pageSize)
        {
            var q = _context.Charities.Include(c => c.ScrapedInfo).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(c => c.Name.Contains(term) || c.Description.Contains(term));
            }

            return await q
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Charity?> GetWithCampaignsAsync(int id)
        {
            return await _context.Charities
                .Include(c => c.ScrapedInfo)
                .Include(c => c.Campaigns)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task BulkImportAsync(IEnumerable<Charity> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            _context.Charities.AddRange(items);
            return Task.CompletedTask;
        }
    }
}
