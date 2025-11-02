

using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class FollowRepository : Repository<Follow>, IFollowRepository
    {
        public FollowRepository(Context context) : base(context) { }

        public async Task<List<int>> GetAllFollowedCharitiesByDonorAsync(int donorId)
        {
            return await _context.Follows
             .Where(f => f.donornID == donorId)
             .Select(f => f.charityID)
             .ToListAsync();
        }

        public async Task<List<int>> GetAllFollowersForCharityAsync(int charityId)
        {
            return await _context.Follows
             .Where(f => f.charityID == charityId)
             .Select(f => f.donornID)
             .ToListAsync();
        }

        public async Task<Follow> GetFollowAsync(int donorId, int charityId)
            => await _dbSet.FirstOrDefaultAsync(fl => fl.donornID == donorId && fl.charityID == charityId);


        public async Task<bool> IsFollowedAsync(int donorId, int charityId)
        {

            var res =  await _dbSet.AnyAsync(fl => fl.donornID == donorId && fl.charityID == charityId);
            var nn = res;
            return nn;
        }
    }
}
