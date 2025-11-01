using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class DonorRepository : Repository<Donor>, IDonorRepository
    {
        public DonorRepository(Context context) : base(context) { }

        public async Task<List<int>> GetFollowsAsync(int id)
        {
            return await _context.Follows
             .Where(f => f.donornID == id)
             .Select(f => f.donornID)
             .ToListAsync();
        }

        public async Task<List<int>> GetSubscriptionsAsync(int id)
        {
            return await _context.Subscriptions
             .Where(s => s.DonorID == id)
             .Select(f => f.DonorID)
             .ToListAsync();
        }
    }
}
