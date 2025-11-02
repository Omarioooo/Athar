using AtharPlatform.DTO;

namespace AtharPlatform.Repositories
{
    public interface IReactionRepository : IRepository<Reaction>
    {
        Task<Reaction> GetByDonorAndContent(ReactionDto model);
    }
}