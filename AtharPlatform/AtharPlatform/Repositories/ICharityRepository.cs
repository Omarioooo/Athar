namespace AtharPlatform.Repositories
{
    public interface ICharityRepository : IRepository<Charity>
    {
        /// <summary>
        /// Get all followers id of a charity
        /// </summary>
        Task<List<int>> GetAllFollowersAsync(int id);

        /// <summary>
        /// Get all subscribers id of a charity
        /// </summary>
        Task<List<int>> GetCharitySubscribersAsync(int id);

        /// <summary>
        /// Get the charity based on one of it's campagins
        /// </summary>
        Task<Charity> GetCharityByCampaignAsync(int campaignId);
    }
}