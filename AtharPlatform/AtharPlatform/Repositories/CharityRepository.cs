

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
    }
}
