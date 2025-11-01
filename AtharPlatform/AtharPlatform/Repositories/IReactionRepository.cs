using AtharPlatform.DTOs;

namespace AtharPlatform.Repositories
{
    public interface IReactionRepository : IRepository<Reaction>
    {
        Task<Reaction> GetByDonorAndContent(ReactionDto model);
    }
}