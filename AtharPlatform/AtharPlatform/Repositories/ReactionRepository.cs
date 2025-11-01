
using AtharPlatform.DTOs;

namespace AtharPlatform.Repositories
{
    public class ReactionRepository : Repository<Reaction>, IReactionRepository
    {
        public ReactionRepository(Context context) : base(context)
        {
        }

        public async Task<Reaction> GetByDonorAndContent(ReactionDto model)
        {
            var reaction = await _context.Reactions
                 .FirstOrDefaultAsync(r => r.DonorID == model.donorId && r.ContentID == model.contentId);

            return reaction;

        }
    }
}
