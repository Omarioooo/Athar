using AtharPlatform.DTO;

namespace AtharPlatform.Repositories
{
    public interface IReactionRepository : IRepository<Reaction>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="donorId"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        Task<Reaction> GetReactionByDonorAndContentAsync(int donorId, int contentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="donorId"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        Task<bool> IsReactedAsync(int donorId, int contentId);
    }
}