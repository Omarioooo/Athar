

using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class CharityRepository : Repository<Charity>, ICharityRepository
    {
        public CharityRepository(Context context) : base(context) { }

        public async Task<List<int>> GetAllFollowersAsync(int id)
        {
            var donorIds = await _context.Follows
                .Where(f => f.charityID == id)
                .Select(f => f.donornID)
                .ToListAsync();

            return donorIds;
        }

        public async Task<List<int>> GetCharitySubscribersAsync(int id)
        {
            var donorIds = await _context.Subscriptions
                .Where(f => f.CharityID == id)
                .Select(f => f.DonorID)
                .ToListAsync();

            return donorIds;
        }

        public async Task<Charity> GetCharityByCampaignAsync(int campaignId)
        {
            var charity = await _context.Charities
                .Include(c => c.campaigns)
                .FirstOrDefaultAsync(c => c.campaigns.Any(cm => cm.Id == campaignId));

            return charity;
        }
    }
}
