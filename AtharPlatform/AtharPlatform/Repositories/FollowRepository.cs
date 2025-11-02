

using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class FollowRepository : Repository<Follow>, IFollowRepository
    {
        public FollowRepository(Context context) : base(context) { }

        public async Task<List<int>> GetAllFollowedCharitiesByDonorAsync(int donorId)
        {
            return await _context.Follows
             .Where(f => f.DonorId == donorId)
             .Select(f => f.CharityId)
             .ToListAsync();
        }

        public async Task<List<int>> GetAllFollowersForCharityAsync(int charityId)
        {
            return await _context.Follows
             .Where(f => f.CharityId == charityId)
             .Select(f => f.DonorId)
             .ToListAsync();
        }

        public async Task<Follow> GetFollowByDonorAndCharityAsync(int donorId, int charityId)
            => await _dbSet.FirstOrDefaultAsync(fl => fl.DonorId == donorId && fl.CharityId == charityId);


        public async Task<bool> IsFollowedAsync(int donorId, int charityId)
            => await _dbSet.AnyAsync(fl => fl.DonorId == donorId && fl.CharityId == charityId);
    }
}
