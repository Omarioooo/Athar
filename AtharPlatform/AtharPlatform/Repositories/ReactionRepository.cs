
using AtharPlatform.DTO;
using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class ReactionRepository : Repository<Reaction>, IReactionRepository
    {
        public ReactionRepository(Context context) : base(context) { }

        public async Task<Reaction> GetReactionByDonorAndContentAsync(int donorId, int contentId)
            => await _dbSet.FirstOrDefaultAsync(r => r.DonorID == donorId && r.ContentID == contentId);

        public async Task<bool> IsReactedAsync(int donorId, int contentId)
            => await _dbSet.AnyAsync(r => r.DonorID == donorId && r.ContentID == contentId);
    }
}
